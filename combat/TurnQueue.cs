using Godot;
using System;

public class TurnQueue : Node2D
{
    public Player player;
    public Enemy enemy;

    public int cooldownSpecial = 0;
    public bool playerKill = false;
    // public bool enemyKill = false;

    // Called when the node enters the scene tree for the first time.
    
    public override void _Ready()
    {

    }

    public void setChars(Player player, Enemy enemy)
    {
        this.player = player;
        this.enemy = enemy;
    }

    public int randomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }

    public bool combat()
    {
        if (player.getMaxHp() <= 0)
        {
            // player lost -> back to main menu
            return playerKill = true;
        }
        else if (enemy.getMaxHp() <= 0)
        {
           // player won -> go on playing
           return playerKill = false;
        }
        else
        {
            if (player.getAgility() > enemy.getAgility())
            {
                // player goes first
                // check if attack or special

                playerAttack();
                enemyAttack();

                if (cooldownSpecial >= 1)
                {
                    cooldownSpecial --;
                }
                
                combat();
            }
            else if (player.getAgility() <= enemy.getAgility())
            {
                // enemy goes first
                enemyAttack();
                // check if attack or special
                playerAttack();

                if (cooldownSpecial >= 1)
                {
                    cooldownSpecial --;
                }

                combat();
            }

        }

        return false;
    }

    public void playerAttack()
    {
        if (player.getMaxHp() > 0)
        {
            // attack 1
            int modifier = randomNumber(1, 3);
            int strength = player.getStrength();
            int damage = strength * modifier;
            int enemyHp = enemy.getMaxHp() - damage;
            enemy.setMaxHp(enemyHp);

            GD.Print(player.getMaxHp());
            GD.Print(enemy.getMaxHp());
        }
    }

    public void playerSpecialAttack()
    {
        if (player.getMaxHp() > 0)
        {
            // attack 2 special
            int modifier = randomNumber(3, 5);
            int strength = player.getStrength();
            int damage = strength * modifier;
            int enemyHp = enemy.getMaxHp() - damage;
            enemy.setMaxHp(enemyHp);
            cooldownSpecial = 2;
        }
    }

    public void enemyAttack()
    {
        if (enemy.getMaxHp() > 0)
        {
            int modifier = randomNumber(1, 2);
            int strength = enemy.getStrength();
            int damage = strength * modifier;
            int playerHp = player.getMaxHp() - damage;
            player.setMaxHp(playerHp);

            GD.Print(player.getMaxHp());
            GD.Print(enemy.getMaxHp());
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
