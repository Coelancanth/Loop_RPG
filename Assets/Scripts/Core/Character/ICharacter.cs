using UnityEngine;

public interface ICharacter
{
    float Health { get; }
    float MaxHealth { get; }
    float Attack { get; }
    float Defense { get; }
    float Evasion { get; }
    
    void AddEvasionBonus(float bonus);
    void RemoveEvasionBonus(float bonus);
    void AddDefenseBonus(float bonus);
    void RemoveDefenseBonus(float bonus);
    void TakeDamage(float damage);
    void Heal(float amount);
} 