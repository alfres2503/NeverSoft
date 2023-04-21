using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServicePlanAssignment : IServicePlanAssignment
    {
        IRepositoryPlanAssignment repository = new RepositoryPlanAssignment();
        public PlanAssignment GetPlanAssignmentByID(int id)
        {
            return repository.GetPlanAssignmentByID(id);
        }

        public IEnumerable<PlanAssignment> GetPlanAssignments()
        {
            return repository.GetPlanAssignments();
        }

        public PlanAssignment Save(PlanAssignment planAssignment)
        {
            return repository.Save(planAssignment);
        }
        public PlanAssignment GetPlanAssignmentByMonthAndYear(int month, int year, int IdResidence)
        {
            return repository.GetPlanAssignmentByMonthAndYear(month, year, IdResidence);
        }

        public IEnumerable<PlanAssignment> GetDebtsByResidence(int IdResidence)
        {
            return repository.GetDebtsByResidence(IdResidence);
        }

        public void SetDebtsAsPaid(string[] selectedDebts)
        {
            repository.SetDebtsAsPaid(selectedDebts);
        }

        public void GetMonthlyIncomesOfTheCurrentYear(out string etiquetas1, out string valores1)
        {
            repository.GetMonthlyIncomesOfTheCurrentYear(out string etiquetas, out string valores);
            etiquetas1 = etiquetas;
            valores1 = valores;
        }
    }
}
