public interface IDamageVisitor
{
    void VisitArmored(ArmoredEnemy enemy, float damage);
    void VisitFlying(FlyingEnemy enemy, float damage);
}