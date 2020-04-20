using DataAccess.Extensions;

namespace DataAccess.Model {
    public class BaseModel : DbContext {
        public BaseModel () : base (nameof (BaseModel)) {
            Database.SetInitializer<BaseModel> (null);
        }
    }
}