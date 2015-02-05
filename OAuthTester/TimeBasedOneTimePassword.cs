using System;



/*
 * This module contains two classes with some static methods, all copied from an excellent article called "Implementing Two Factor Authentication in ASP.NET MVC with Google Authenticator"
 * published on 9/17/2012 on the Code Project web-site by Rick Bassham.
 * 
 * This namespace really belongs in it's own project .dll (Just add a reference to the namespace in whatever project uses it), as the source-code is just a liability in a deployed website. 
 */

namespace TwoFactor
{
    public static class TimeBasedOneTimePassword
    {
        public static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string GetPassword(string secret)
        {
            return GetPassword(secret, GetCurrentCounter());
        } // end GetPassword

        public static string GetPassword(string secret, DateTime epoch, int timeStep)
        {
            long counter = GetCurrentCounter(DateTime.UtcNow, epoch, timeStep);

            return HashedOneTimePassword.GeneratePassword(secret, counter);
        } // end GetPassword

        public static string GetPassword(string secret, DateTime now, DateTime epoch, int timeStep, int digits)
        {
            long counter = GetCurrentCounter(now, epoch, timeStep);

            return HashedOneTimePassword.GeneratePassword(secret, counter, digits);
        } // end GetPassword

        private static string GetPassword(string secret, long counter)
        {
            return HashedOneTimePassword.GeneratePassword(secret, counter);
        } // end GetPassword

        private static long GetCurrentCounter()
        {
            return GetCurrentCounter(DateTime.UtcNow, UNIX_EPOCH, 30);
        } // end GetCurrentCounter

        private static long GetCurrentCounter(DateTime now, DateTime epoch, int timeStep)
        {
            return (long)(now - epoch).TotalSeconds / timeStep;
        } // end GetCurrentCounter

        public static bool IsValid(string secret, string password, int checkAdjacentIntervals = 1)
        {
            if (password == GetPassword(secret))
                return true;

            for (int i = 1; i <= checkAdjacentIntervals; i++)
            {
                if (password == GetPassword(secret, GetCurrentCounter() + i))
                    return true;

                if (password == GetPassword(secret, GetCurrentCounter() - i))
                    return true;
            }

            return false;
        } // end IsValid
    } // end TimeBasedOneTimePassword class

} // end TwoFactor namespace