using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InsuranceManagement.API.Endpoints;

/// <summary>Maps the beginner-friendly in-memory demo item routes.</summary>
public static class DemoItemEndpoints
{
    /// <summary>Maps read and create routes for demo items.</summary>
    public static IEndpointRouteBuilder MapDemoItemEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var items = endpoints.MapGroup("/api/demo/items")
            .WithTags("Demo items")
            .AllowAnonymous();

        items.MapGet("/", (DemoItemStore store) =>
                TypedResults.Ok(store.GetAll()))
            .WithName("GetDemoItems")
            .WithSummary("Get all demo items")
            .WithDescription("Returns the items currently held in the API process's in-memory collection.")
            .Produces<IReadOnlyList<DemoItemResponse>>(StatusCodes.Status200OK);

        items.MapGet("/{id:int}", Results<Ok<DemoItemResponse>, NotFound> (
                int id,
                DemoItemStore store) =>
            {
                var item = store.GetById(id);
                return item is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(item);
            })
            .WithName("GetDemoItemById")
            .WithSummary("Get a demo item by ID")
            .WithDescription("Returns one in-memory demo item, or 404 when the ID does not exist.")
            .Produces<DemoItemResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        items.MapPost("/", Results<CreatedAtRoute<DemoItemResponse>, ValidationProblem> (
                CreateDemoItemRequest request,
                DemoItemStore store) =>
            {
                var name = request.Name?.Trim();
                if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
                {
                    return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                    {
                        ["name"] = ["Name is required and must be 100 characters or fewer."]
                    });
                }

                var item = store.Create(name);
                return TypedResults.CreatedAtRoute(
                    item,
                    routeName: "GetDemoItemById",
                    routeValues: new { id = item.Id });
            })
            .WithName("CreateDemoItem")
            .WithSummary("Create a demo item")
            .WithDescription("Adds an item to the in-memory collection and returns its resource URL.")
            .Produces<DemoItemResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return endpoints;
    }
}
