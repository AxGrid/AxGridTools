using System;
using System.Collections.Generic;
using System.IO;
using AxGrid.Text;
using AxGrid.Utils;
using NUnit.Framework;
using SmartFormat;
using YamlDotNet.Serialization;

namespace AxGridToolsTest
{
    [TestFixture]
    public class TextTest
    {
        private static string textRu = @"
app:
  formats:
    wheelBonusWin: '{WheelBonusWin:tp(app.templates.cost)}'
    win: '{Win:tp(app.templates.balance)}'
    winCost: '{Win:tp(app.templates.cost)}'
    winnings: '{Winnings:tp(app.templates.balance)}'
    winningsCost: '{Winnings:tp(app.templates.cost)}'
  main:
    shop: 'Магазин'
    rank: 'Рейтинг'
    friends: 'Друзья'
    add100Coins: 'МОНЕТ'
    btnOnLine: 'ОНЛАЙН'
    btnFriends: ""С ДРУЗЬЯМИ""
    btnSingle: 'ОДИНОЧНАЯ'
  promo:
    quest:
      questTitle: 'Задание'
      boost: 'БОНУС'
      limitedOffer: 'ОГРАНИЧЕННОЕ ПРЕДЛОЖЕНИЕ'
      smartFormat: '{0:фишку|фишки|фишек}'
";


        private static string textEn = @"
app:
  main:
    shop: 'Shop'
    rank: 'Rank'
    tournament: 'Tournament'
";
        
        [Test]
        public void TestYamlLoad()
        {
            var deserializer = new Deserializer();
            var obj = (Dictionary<object, object>) deserializer.Deserialize(
                new StringReader(textRu),
                typeof(Dictionary<object, object>)
            );
            
            Assert.NotNull(obj);
            
            var res = obj.FlattenKeys();
            foreach (var keyValuePair in res)
            {
                Console.WriteLine($"{keyValuePair.Key} == {keyValuePair.Value}");
            }
            
            Assert.True(res.ContainsKey("app.formats.wheelBonusWin"));
            Assert.AreEqual(res["app.formats.wheelBonusWin"], "{WheelBonusWin:tp(app.templates.cost)}");
            Assert.True(res.ContainsKey("app.main.btnFriends"));
            Assert.AreEqual(res["app.main.btnSingle"], "ОДИНОЧНАЯ");
        }

        [Test]
        public void TestCreateRepository()
        {
            TextRepository tr = new TextRepository(new[] {textRu, textEn});
            Assert.AreEqual(tr.Translations["app.main.shop"], "Магазин");
            Assert.AreEqual(tr.Translations["app.main.tournament"], "Tournament");
            
            tr = new TextRepository(new[] {textEn, textRu});
            Assert.AreEqual(tr.Translations["app.main.shop"], "Shop");
            Assert.AreEqual(tr.Translations["app.main.tournament"], "Tournament");
        }


        [Test]
        public void TestStringFormat()
        {
            var tr = new TextRepository(new[] {textRu, textEn});
            Assert.AreEqual(tr.Translations["app.main.shop"], "Магазин");
            Assert.AreEqual(tr.Get("app.main.shop"), "Магазин");
        }
    }
}