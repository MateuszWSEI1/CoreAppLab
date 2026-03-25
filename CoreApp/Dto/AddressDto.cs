using CoreApp.Enums;

namespace CoreApp.Dto;

public record AddressDto(
    string Street,
    string City,
    string PostalCode,
    string Country,
    AddressType Type
);