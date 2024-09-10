﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Collaborators;
public class GetCollaboratorsUseCase(ICollaboratorRepository collaboratorRepository)
{
    public async Task<List<Collaborator>> Execute()
    {
        return await collaboratorRepository.GetAll();
    }
}
