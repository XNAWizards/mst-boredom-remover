namespace mst_boredom_remover.engine
{
    class Order
    {
        public enum OrderType
        {
            Move,
            Attack,
            Follow,
            AttackMove,
            Patrol,
            Gather,
            Gaurd,
            Idle,
            Build,
            Produce
        };
        public OrderType orderType;
        public Position targetPosition; // Can be Null
        public Unit targetUnit; // Can be Null
        public UnitType unitTypeBuild; // Nullable

        public static Order CreateMoveOrder(Position position)
        {
            return new Order()
            {
                orderType = OrderType.Move,
                targetPosition = position,
                targetUnit = null,
                unitTypeBuild = null
            };
        }

        public static Order CreateProduceOrder(UnitType unitType)
        {
            return new Order()
            {
                orderType = OrderType.Produce,
                targetPosition = null,
                targetUnit = null,
                unitTypeBuild = unitType
            };
        }

        public static Order CreateAttackOrder(Unit target)
        {
            return new Order()
            {
                orderType = OrderType.Attack,
                targetUnit = target,
                targetPosition = target.position,
                unitTypeBuild = null
            };
        }

        public static Order CreateGatherOrder(Position target)
        {
            return new Order()
            {
                orderType = OrderType.Gather,
                targetUnit = null,
                targetPosition = target,
                unitTypeBuild = null
            };
        }
    }
}
