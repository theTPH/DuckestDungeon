using Godot;
using System;

public class TurnQueue : Node2D
{
    public Player player;
    public Enemy enemy;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetNode<Player>("Player");
        enemy = GetNode<Enemy>("Enemy");
    }

    public int randomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }

    public void combat()
    {
        if (player.getMaxHp() <= 0)
        {
            // destroy playernode -> back to main menu
        }
        else if (enemy.getMaxHp() <= 0)
        {
           // destroy enemynode -> go on playing
        }
        else
        {
            if (player.getAgility() > enemy.getAgility())
            {
                // player goes first
                // check if attack or special
                playerAttack();
                enemyAttack();
            }
            else if (player.getAgility() <= enemy.getAgility())
            {
                // enemy goes first
                enemyAttack();
                // check if attack or special
                playerAttack();
            }

            combat();
        }
    }

    public void playerAttack()
    {
        // attack 1
        int modifier = randomNumber(1, 3);
        int strength = player.getStrength();
        int damage = strength * modifier;
        int enemyHp = enemy.getMaxHp() - damage;
        enemy.setMaxHp(enemyHp);
    }

    public void playerSpecialAttack()
    {
        // attack 2 special
        int modifier = randomNumber(3, 5);
        int strength = player.getStrength();
        int damage = strength * modifier;
        int enemyHp = enemy.getMaxHp() - damage;
        enemy.setMaxHp(enemyHp);
    }

    public void enemyAttack()
    {
        int modifier = randomNumber(1, 2);
        int strength = enemy.getStrength();
        int damage = strength * modifier;
        int playerHp = player.getMaxHp() - damage;
        player.setMaxHp(playerHp);
    }

    public void onAttackBtnClick()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
