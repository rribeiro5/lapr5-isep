using System;
using System.Collections.Generic;
using DDDSample1.Domain.Users;

namespace DDDSample1.Utils
{
    public abstract class LeaderboardComparer
    {
        public static Comparer<UserLeaderboardDTO> Get()
        {
            return Comparer<UserLeaderboardDTO>.Create(
                (x,y) =>
                {
                    var result=y.value.CompareTo(x.value);
                    if (result == 0)
                    {
                        result = string.Compare(x.Name,y.Name,StringComparison.Ordinal);
                    }
                    if (result == 0)
                    {
                        result=string.Compare(x.Email, y.Email, StringComparison.Ordinal);
                    }
                    return result;
                });
        }
    }
}