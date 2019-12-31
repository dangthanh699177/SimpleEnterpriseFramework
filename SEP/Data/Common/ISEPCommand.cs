using System.Threading.Tasks;

namespace SEP.Data.Common
{
    public interface ISEPCommand
    {
        Task<int> Delete();
        Task<int> Insert();
        Task<int> Update();
    }
}