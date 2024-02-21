using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Multis;
using Server.Prompts;
using Server.Targeting;
using Server.Accounting;
using System.Linq;

using System.Diagnostics; 
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; 


namespace Server.Mobiles
{
    public class SimpleChatVendor : Mobile
    {

        private static readonly HttpClient client = new HttpClient();
        
        private static readonly string PythonPath = "/usr/bin/python";
        private static readonly string ChatScriptPath = "/opt/ServUO/Scripts/Custom/query_chatgpt.py";

        public async void CallChatGPT(string query, Mobile from)
    {
        var payload = new
        {
            query = query
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync("http://localhost:5000/chatgpt", content);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                string chatGptResponse = responseObject.response; 
                this.Say(chatGptResponse);

            }
            else
            {
                from.SendMessage("Desculpe, não consegui obter uma resposta.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao chamar o ChatGPT: {ex.Message}");
        }
    }



        [Constructable]
        public SimpleChatVendor()
        {
            Name = "ChatGPT";
            if (!Core.AOS)
               NameHue = 0x35;

            InitStats(100, 100, 100);
            InitBody();
            InitOutfit();
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            Mobile from = e.Mobile;

            if (!e.Handled && from.InRange(this.Location, 3))
            {
                if (e.Speech.ToLowerInvariant().Contains("chat"))
                {
                    string query = e.Speech.Substring(e.Speech.IndexOf("chat") + "chat".Length).Trim();
                    CallChatGPT(query, from);
                    e.Handled = true;
                }
            }
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 3))
                return true;

            return base.HandlesOnSpeech(from);
        }

        public void CallChatGPT(string query)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = PythonPath,
                Arguments = $"{ChatScriptPath} \"{query}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }

        public void InitBody()
        {
            Hue = Utility.RandomSkinHue();
            SpeechHue = 0x3B2;

            if (!Core.AOS)
                NameHue = 0x35;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }
        }

        public virtual void InitOutfit()
        {
            Item item = new FancyShirt(Utility.RandomNeutralHue());
            item.Layer = Layer.InnerTorso;
            AddItem(item);
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new BodySash(Utility.RandomNeutralHue()));
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));

            Utility.AssignRandomHair(this);

            Container pack = new VendorBackpack();
            pack.Movable = false;
            AddItem(pack);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // Versão
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public SimpleChatVendor(Serial serial) : base(serial)
        {
        }


        public static void Configure()
        {
        }
    }
}
