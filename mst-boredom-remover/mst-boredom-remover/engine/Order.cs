using System.Diagnostics;

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
        public readonly OrderType orderType;
        public readonly Position targetPosition; // Can be Null
        public readonly Unit targetUnit; // Can be Null
        public readonly UnitType unitTypeBuild; // Nullable

        public Order(OrderType orderType, Position targetPosition=null, Unit targetUnit=null, UnitType unitTypeBuild=null)
        {
            this.orderType = orderType;
            this.targetPosition = targetPosition;
            this.targetUnit = targetUnit;
            this.unitTypeBuild = unitTypeBuild;
        }

        public static Order CreateMoveOrder(Position position)
        {
            Debug.Assert(position != null);
            return new Order(OrderType.Move, targetPosition: position);
        }

        public static Order CreateProduceOrder(UnitType unitType, Position targetPosition)
        {
            Debug.Assert(unitType != null);
            return new Order(OrderType.Produce, targetPosition: targetPosition, unitTypeBuild: unitType);
        }

        public static Order CreateAttackOrder(Unit target)
        {
            Debug.Assert(target != null);
            return new Order(OrderType.Attack, targetUnit: target, targetPosition: target.position);
        }

        public static Order CreateGatherOrder(Position targetPosition)
        {
            Debug.Assert(targetPosition != null);
            return new Order(OrderType.Gather, targetPosition: targetPosition);
        }
    }
}
