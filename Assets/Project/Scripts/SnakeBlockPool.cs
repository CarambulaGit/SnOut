namespace Project.Scripts {
    public class SnakeBlockPool : Pool<SnakeBlockView> {
        public override void ReturnObject(SnakeBlockView obj) {
            obj.Disconnect();
            base.ReturnObject(obj);
        }
    }
}