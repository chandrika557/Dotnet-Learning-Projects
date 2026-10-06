using AutoMapper;
using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.Services.Mapping;

/// <summary>Defines mappings between persistence entities and API request/response types.</summary>
public sealed class InsuranceMappingProfile : Profile
{
    public InsuranceMappingProfile()
    {
        CreateMap<Customer, CustomerResponse>();
        CreateMap<CreateCustomerRequest, Customer>();
        CreateMap<UpdateCustomerRequest, Customer>();

        CreateMap<Policy, PolicyResponse>();
        CreateMap<CreatePolicyRequest, Policy>();
        CreateMap<UpdatePolicyRequest, Policy>();

        CreateMap<Claim, ClaimResponse>();
        CreateMap<CreateClaimRequest, Claim>();
    }
}
