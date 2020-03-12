using Godot;
using System;

public class TurnQueue : Node2D
{
    private Player player;
    private Enemy enemy;

    private int cooldownSpecial = 0;
    public bool PlayerKill = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // get player and enemy nodes from room scene
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

    // method to start combat (beta)
    public bool combat()
    {
        if (player.getMaxHp() <= 0)
        {
            // player lost -> back to main menu
            return PlayerKill = true;
        }
        else if (enemy.getMaxHp() <= 0)
        {
           // player won -> go on playing
           return PlayerKill = false;
        }
        else
        {
            if (player.getAgility() > enemy.getAgility())
            {
                // player goes first
                //player.Position = new Vector2(800, 380);
                playerAttack();
                //player.Position = new Vector2(580, 380);
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

    // methods to declare attacks from both sides
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

            System.Threading.Thread.Sleep(1000);

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

            System.Threading.Thread.Sleep(1000);

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
