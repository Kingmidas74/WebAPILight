using Contracts.Shared.Results;
using DataAccess.Extensions;
using DataAccess.Extensions.StoredProcAttributes;
using DataAccess.Interfaces;
using Domain.Parameters;

namespace DataAccess.Model {
    public class ParentModel : BaseModel, IModel<Parent> {
        [Name ("admin." + nameof (Parent) + "_" + nameof (GetAllPaged))]
        [ReturnTypes (typeof (Parent))]
        public StoredProcedure<GetAllPagedParameter> GetAllPaged { get; set; }

        [Name ("admin." + nameof (Parent) + "_" + nameof (GetOne))]
        [ReturnTypes (typeof (Parent))]
        public StoredProcedure<GetOneParameter> GetOne { get; set; }

        [Name ("admin." + nameof (Parent) + "_" + nameof (BindTariffPlan))]
        [ReturnTypes (typeof (Parent))]
        public StoredProcedure<BindTariffPlanParameter> BindTariffPlan { get; set; }

        [Name ("admin." + nameof (Parent) + "_" + nameof (IrreversibleDelete))]
        [ReturnTypes (typeof (Parent))]
        public StoredProcedure<IrreversibleDeleteParameter> IrreversibleDelete { get; set; }

        [Name ("admin." + nameof (Parent) + "_" + nameof (BindSubscription))]
        [ReturnTypes (typeof (Parent))]
        public StoredProcedure<BindSubscriptionParameter> BindSubscription { get; set; }

    }
}