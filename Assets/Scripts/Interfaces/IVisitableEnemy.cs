public interface IVisitableEnemy
{
    void Accept(IDamageVisitor visitor, int damage);
}