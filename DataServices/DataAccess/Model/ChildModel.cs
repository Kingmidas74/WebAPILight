using Contracts.Shared.Results;
using DataAccess.Extensions;
using DataAccess.Extensions.StoredProcAttributes;
using DataAccess.Interfaces;
using Domain.Parameters;

namespace DataAccess.Model {
    public class ChildModel : BaseModel, IModel<Child> {
        [Name ("admin." + nameof (Child) + "_" + nameof (GetAllByParent))]
        [ReturnTypes (typeof (Child))]
        public StoredProcedure<GetAllByParentParameter> GetAllByParent { get; set; }
    }
}