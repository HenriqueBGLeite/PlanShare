using PlanShare.App.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanShare.App.UseCases.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        Task<Result> Execute(Models.Login model);
    }
}
