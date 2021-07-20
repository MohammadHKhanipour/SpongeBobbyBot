using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SpongeBobbyBot
{
    /*
     * Prototype with really dirty code! :D
     * Will make it better later
    */

    class Program
    {
        static TelegramBotClient BotClient = new TelegramBotClient("TOKEN");

        static void Main(string[] args)
        {
            BotClient.StartReceiving();
            BotClient.OnMessage += BotClient_OnMessage;

            Console.ReadLine();
        }

        private async static void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                Random rand = new Random();

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "Script.txt";
                List<string> lines = new List<string>();
                lines = File.ReadAllLines(filePath).OrderBy(x => rand.Next()).ToList();

                if (e.Message.Text == "/start")
                {
                    string message = "Hello and welcome! Use /help to see a list of all commands.";
                    await BotClient.SendTextMessageAsync(e.Message.Chat.Id, message);

                    Console.WriteLine($"{DateTime.Now}: Start\n");
                }

                if (e.Message.Text == "/help")
                {
                    string message = "So far there are 3 types of commands\n(other than /start and /help):\n\n• <code>don_n</code> - Sends you <b>n</b> lines of transcript in separate messages.\n<b>Example:</b> /don_3 will send you 3 lines of transcript in <b>3</b> messages.\n\n• <code>dosdn_n_t</code> - This one is just like <code>don_n</code> but using this command you can also set the delay between each message. Sends you <b>n</b> lines of transcript with <b>t</b> second(s) of delay between messges.\n<b>Example:</b> /dosdn_2_6 will send you <b>2</b> messages with <b>6</b> seconds of delay in between.\n<i>Note that you can use decimal numbers to determine the delay as well.</i>\n\n• <code>dot_t</code> - This command will start sending messages for <b>t</b> seconds. <b>[BETA]</b> \n<b>Example:</b> /dot_20 will send messages for <b>20</b> seconds.\n\nThese commands are provided in command list for quick access";
                    await BotClient.SendTextMessageAsync(e.Message.Chat.Id, message, Telegram.Bot.Types.Enums.ParseMode.Html);

                    Console.WriteLine($"{DateTime.Now}: Help\n");
                }

                if (e.Message.Text.StartsWith("/don"))
                {
                    try
                    {
                        int i = 0;
                        int to = Convert.ToInt32(e.Message.Text.Split('_')[1]);

                        Console.WriteLine($"{DateTime.Now}: Do {to} Times\n");

                        foreach (var item in lines)
                        {
                            if (i >= to)
                                break;
                            await BotClient.SendTextMessageAsync(e.Message.Chat.Id, item);
                            Thread.Sleep(2000);
                            i++;
                        }

                        await BotClient.SendTextMessageAsync(e.Message.Chat.Id, "FINISHED!");
                    }
                    catch (Exception)
                    {
                        await BotClient.SendTextMessageAsync(e.Message.Chat.Id, "Something went wrong!");
                    }
                }

                if (e.Message.Text.StartsWith("/dosdn"))
                {
                    try
                    {
                        int i = 0;
                        int to = Convert.ToInt32(e.Message.Text.Split('_')[1]);
                        int time = Convert.ToInt32(Convert.ToDouble(e.Message.Text.Split('_')[2]) * 1000);

                        Console.WriteLine($"{DateTime.Now}: Do {to} Times with {time} Milliseconds of Delay\n");

                        foreach (var item in lines)
                        {
                            if (i >= to)
                                break;
                            await BotClient.SendTextMessageAsync(e.Message.Chat.Id, item);
                            Thread.Sleep(time);
                            i++;
                        }

                        await BotClient.SendTextMessageAsync(e.Message.Chat.Id, "FINISHED!");
                    }   
                    catch (Exception)
                    {
                        await BotClient.SendTextMessageAsync(e.Message.Chat.Id, "Something went wrong!");
                    }
                }

                if (e.Message.Text.StartsWith("/dot"))
                {
                    try
                    {
                        int i = 0;
                        int timeSpan = Convert.ToInt32(e.Message.Text.Split('_')[1]);

                        Console.WriteLine($"{DateTime.Now}: Do for {timeSpan} Seconds\n");

                        foreach (var item in lines)
                        {
                            if (i >= timeSpan)
                                break;
                            await BotClient.SendTextMessageAsync(e.Message.Chat.Id, item);
                            Thread.Sleep(1000);
                            i++;
                        }

                        await BotClient.SendTextMessageAsync(e.Message.Chat.Id, "FINISHED!");
                    }
                    catch (Exception)
                    {
                        await BotClient.SendTextMessageAsync(e.Message.Chat.Id, "Something went wrong!");
                    }
                }

                if (e.Message.Text.EndsWith("@SpongeBobbyBot"))
                {
                    await BotClient.SendTextMessageAsync(e.Message.Chat.Id, "Wrong format, bot doesn't need to be mentioned");
                }
            }
            else
            {
                await BotClient.SendTextMessageAsync(e.Message.Chat.Id, "Not Supported (Yet!)");
                Console.WriteLine($"{DateTime.Now}: Unsopported\n");
            }
        }
    }
}