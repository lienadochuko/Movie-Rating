﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Movie_Rating.Models.DTO;


namespace Movie_Rating.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating actor entity
    /// </summary>
    public interface IMoviesUpdaterServices
    {
        Task<bool> UpdateActors(ActorsUpdateRequest actorsUpdateRequest, CancellationToken cancellationToken);
    }
}
