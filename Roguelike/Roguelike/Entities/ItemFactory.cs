using Roguelike.Entities.Components;
using Roguelike.Render;
using System.Drawing;

namespace Roguelike.Entities
{
    public static class ItemFactory
    {
        public static Entity CreatePotion(int x, int y, int dungeonLevel)
        {
            var potion = new Entity("Healing Potion", x, y, '!', Color.Violet)
            {
                SpriteIndex = EntitySprites.Potion,
                RenderLayer = Renderer.ItemLayer
            };

            potion.AddComponent(new ItemComponent { Description = "Heals you for a small amount of HP.", UseFunction = ItemFunctions.PotionFunction });

            return potion;
        }

        public static Entity CreateLightningScroll(int x, int y, int dungeonLevel)
        {
            var lightningScroll = new Entity("Lightning Scroll", x, y, '[', Color.LightBlue)
            {
                SpriteIndex = EntitySprites.Scroll,
                SpriteTint = Color.LightBlue,
                RenderLayer = Renderer.ItemLayer
            };

            lightningScroll.AddComponent(new ItemComponent { Description = "Fires bolts of lightning at your enemies.", UseFunction = ItemFunctions.LightningScroll });

            return lightningScroll;
        }

        public static Entity CreateConfuseScroll(int x, int y, int dungeonLevel)
        {
            var confuseScroll = new Entity("Confuse Scroll", x, y, '[', Color.LightYellow)
            {
                SpriteIndex = EntitySprites.Scroll,
                SpriteTint = Color.LightYellow,
                RenderLayer = Renderer.ItemLayer
            };

            confuseScroll.AddComponent(new ItemComponent { Description = "Complicated math formulas that confuse your enemies.", UseFunction = ItemFunctions.ConfuseScroll });

            return confuseScroll;
        }

        public static Entity CreateFireballScroll(int x, int y, int dungeonLevel)
        {
            var fireballScroll = new Entity("Fireball Scroll", x, y, '[', Color.Orange)
            {
                SpriteIndex = EntitySprites.Scroll,
                SpriteTint = Color.Orange,
                RenderLayer = Renderer.ItemLayer
            };

            fireballScroll.AddComponent(new ItemComponent { Description = "Launch a ball of fire at your enemies.", UseFunction = ItemFunctions.FireballScroll });

            return fireballScroll;
        }

        public static Entity CreateStairs(int x, int y, int dungeonLevel)
        {
            var stairs = new Entity("Stairs", x, y, '<', Color.White)
            {
                SpriteIndex = EntitySprites.Stairs,
                SpriteTint = Color.White,
                RenderLayer = Renderer.FeaturesLayer,
                IsAlwaysVisible = true
            };

            stairs.Tags.Add(Tags.Stairs);

            return stairs;
        }

        public static Entity CreateSword(int x, int y, int dungeonLevel)
        {
            var sword = new Entity("Sword", x, y, '/', Color.Silver)
            {
                SpriteIndex = EntitySprites.Sword,
                SpriteTint = Color.White,
                RenderLayer = Renderer.ItemLayer
            };

            sword.AddComponent(new ItemComponent());
            sword.AddComponent(new EquipmentComponent(EquipmentSlot.MainHand) { Power = 2 });

            return sword;
        }
    }
}
