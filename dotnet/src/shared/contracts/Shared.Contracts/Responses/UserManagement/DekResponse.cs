namespace Shared.Contracts.Responses.UserManagement;

public sealed record DekResponse(string ClientSalt, string EncryptedDek, int CryptoVersion, string Login);