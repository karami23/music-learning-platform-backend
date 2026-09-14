namespace MusicEducation.Application.Queries.Identity.User.GetUserByPhoneNumber;

public sealed record GetUserByPhoneNumberQuery(
    string CountryCode,
    string NationalNumber
);