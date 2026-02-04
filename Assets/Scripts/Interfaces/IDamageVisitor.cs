public interface IDamageVisitor
{
    void VisitArmored(ArmoredEnemy enemy, int damage);
    void VisitFlying(FlyingEnemy enemy, int damage);
}