
public string GenAccessToken(int userId)
{
  var tokenHandle = new JwtSecurityTokenHandler();
  var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
  var tokenDescription = new SecurityTokenDescriptor
  {
    Subject = new ClaimsIdentity([
      new Claim(ClaimTypes.NameIdentifier,userId.ToString())
    ]),
    Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:AccessTokenExpirationMinutes"])),
    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
    Issuer = configuration["Jwt:Issuer"],
    Audience = configuration["Jwt:Audience"],
  };
  var token = tokenHandle.CreateToken(tokenDescription);
  return tokenHandle.WriteToken(token);
}

public string GenRefreshToken()
{
  var randomNumber = new byte[32];
  using (var rng = RandomNumberGenerator.Create())
  {
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }
}

//=========================
// old
public string GenAccessToken(string username)
{
  var secrectKey = configuration["token:SecrectKey"];
  var secrectKeyEncoding = Encoding.UTF8.GetBytes(secrectKey);
  var myClaims = new List<Claim>{
    new Claim(JwtRegisteredClaimNames.Sub, username),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
  };
  var tokenDescription = new SecurityTokenDescriptor()
  {
    Subject = new ClaimsIdentity(myClaims),
    Expires = DateTime.UtcNow.AddMinutes(1),
    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secrectKeyEncoding), SecurityAlgorithms.HmacSha256)

  };
  var MyJwt = new JwtSecurityTokenHandler();
  var token = MyJwt.CreateToken(tokenDescription);
  return MyJwt.WriteToken(token);
}