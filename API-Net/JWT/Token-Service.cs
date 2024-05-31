    public TokenServiceImpl(ApiExampleContext _db, IConfiguration _configuration)
    {
      db = _db;
      configuration = _configuration;
    }



    public async Task<TokenDto> GetTokenAsync(string refreshToken)
    {
      var token = await db.Tokens.SingleOrDefaultAsync(t => t.RefreshToken == refreshToken);
      if (token is null) return null;
      return new TokenDto
      {
        AccessToken = token.AccessToken,
        RefreshToken = token.RefreshToken,
        ExpiryDate = token.ExpiryDate.ToString("dd/MM/yyyy")
      };
    }
    public async Task<string?> RefreshTokenAsync(string refreshToken)
    {
      var token = await db.Tokens.SingleOrDefaultAsync(t => t.RefreshToken == refreshToken);
      if (token is null) return null;

      if (token.ExpiryDate < DateTime.UtcNow)
      {
        throw new BadRequestException(401, "refreshtoken has expired");
      }

      var newAccessToken = GenAccessToken(token.EmployeeId);
      token.AccessToken = newAccessToken;
      db.Entry(token).State = EntityState.Modified;
      await db.SaveChangesAsync();

      return newAccessToken;
    }

    public async Task<bool> SaveTokenAsync(int userId, string accessToken, string refreshToken)
    {

      var token = new Token
      {
        EmployeeId = userId,
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        ExpiryDate = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:RefreshTokenExpirationMinutes"]))
      };
      try
      {
        var FindToken = await db.Tokens.SingleOrDefaultAsync(t => t.EmployeeId == userId);
        if (FindToken == null)
        {
          await db.Tokens.AddAsync(token);
        }
        else
        {
          FindToken.AccessToken = token.AccessToken;
          FindToken.RefreshToken = token.RefreshToken;
          FindToken.ExpiryDate = token.ExpiryDate;
          db.Entry(FindToken).State = EntityState.Modified;
        }
        return await db.SaveChangesAsync() > 0;

      }
      catch
      {
        return false;
      }

    }
    public async Task<bool> DeleteTokenAsync(Token token)
    {
      try
      {
        db.Tokens.Remove(token);
        return await db.SaveChangesAsync() > 0;
      }
      catch
      {
        return false;
      }
    }
