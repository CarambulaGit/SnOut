using Project.Classes;

namespace Project.Scripts {
    public class BlocksPool : Pool<BlockView> {
        public override void ReturnObject(BlockView obj) {
            obj.Disconnect();
            base.ReturnObject(obj);
        }
    }
}