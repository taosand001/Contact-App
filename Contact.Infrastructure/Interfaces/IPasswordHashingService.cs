﻿namespace Contact.Infrastructure.Interfaces
{
    public interface IPasswordHashingService
    {
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}