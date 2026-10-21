using System.Text.Json.Serialization;

namespace NorthWind.Membership.Entities.Dtos.UserLogin;

public class TokensDto
{
    public TokensDto() { }

    [JsonConstructor]
    public TokensDto(string accessToken)
    {
        AccessToken = accessToken;
    }

    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;
}
