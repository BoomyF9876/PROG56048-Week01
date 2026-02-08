public interface IVisitableEnemy
{
    void Accept(IDamageVisitor visitor, float damage);
}