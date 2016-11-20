using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class NewGame : Area {
    public Page QuizIntro;
    public Page[] Quiz;
    public Page ChooseName;
    public Page AttAssign;
    private Page primary;

    private int quizIndex;
    private IDictionary<AttributeType, int> points;

    private PlayerCharacter pc;

    private Camp camp;

    public NewGame(Page primary) {
        this.primary = primary;
        this.points = new Dictionary<AttributeType, int>();

        pc = new Hero(new Inventory(), 1, 1, 1, 1);

        foreach (AttributeType at in pc.Attributes.Keys) {
            points.Add(at, 0);
        }

        camp = new Camp(pc);
    }

    public override void Init() {

        QuizIntro = new ReadPage(
            "Before you begin your adventure, you must answer some questions.",
            tooltip: "Are you ready?",
            musicLoc: "Bleeding_Out",
            buttonables: new Process[] {
                new Process("Yes", "Begin the quiz.",
                () => {
                    Page = Quiz[0];
                }),
                new Process("No", "Return to main menu.",
                () => {
                    Page = primary;
                })
            }
            );

        Question[] pool1 = new Question[] {
            new Question(
                "What is your favorite color?",
                new Answer(Util.Color("Red", Color.red), str: 1),
                new Answer(Util.Color("Blue", Color.blue), intel: 1),
                new Answer(Util.Color("Green", Color.green), dex: 1),
                new Answer(Util.Color("Yellow", Color.yellow), vit: 1)
                ),
            new Question(
                "What is your favorite attribute?",
                new Answer("Strength", str: 1),
                new Answer("Intelligence", intel: 1),
                new Answer("Dexterity", dex: 1),
                new Answer("Vitality", vit: 1)
                ),
            new Question(
                "What is your favorite letter?",
                new Answer("S", str: 1),
                new Answer("I", intel: 1),
                new Answer("D", dex: 1),
                new Answer("V", vit: 1)
                ),
            new Question(
                "?",
                new Answer("!", str: 1),
                new Answer("@", intel: 1),
                new Answer("^", dex: 1),
                new Answer("<3", vit: 1)
                ),
            new Question(
                "What is your favorite item?",
                new Answer("Sword", str: 1),
                new Answer("Staff", intel: 1),
                new Answer("Boots", dex: 1),
                new Answer("Shield", vit: 1)
                ),
            new Question(
                "What is your favorite random assortment of characters?",
                new Answer("+=>", str: 1),
                new Answer("[X]", vit: 1),
                new Answer("[_>", dex: 1),
                new Answer("===@", intel: 1)
                ),
            new Question(
                "How do you feel?",
                new Answer(">:=(", str: 1),
                new Answer("8^)", intel: 1),
                new Answer("orz", dex: 1),
                new Answer("._.", vit: 1)
                ),
            new Question(
                "Tabs or spaces?",
                new Answer("wat", str: 1),
                new Answer("Tabs", dex: 1),
                new Answer("Spaces", vit: 1),
                new Answer("Both", intel: 1)
                )
        };
        Question[] pool2 = new Question[] {
            new Question(
                "You face an undefeatable foe!\nWhat do you do?",
                new Answer("Fight", str: 2),
                new Answer("Flee", vit: 2)
                ),
            new Question(
                "What is your preferred combat style?",
                new Answer("Aggressive", str: 2),
                new Answer("Casting spells", intel: 2),
                new Answer("A bit of everything", dex: 2),
                new Answer("I'm a pacifist", vit: 2),
                new Answer("Eradicate them", str: 2),
                new Answer("Controlled"),
                new Answer("DefenCive", vit: 2),
                new Answer("Accurate", str: 2),
                new Answer("DefenSive", vit: 2)
                ),
            new Question(
                "Your enemy lies defeated before you.\nWhat do you do?",
                new Answer("Kill them", str: 2),
                new Answer("Leave them", vit: 2)
                ),
            new Question(
                "Which element resonates strongly with you?",
                new Answer("Fire", str: 2),
                new Answer("Water", intel: 2),
                new Answer("Wind", dex: 2),
                new Answer("Earth", vit: 2)
            ),
            new Question(
                "Do you enjoy wielding two-handed swords?",
                new Answer("Yes", str: 2),
                new Answer("No", vit: 2)
                ),
            new Question(
                "Two of your friends are fighting with each other.\nWhat do you do?",
                new Answer("Talk them out of it", intel: 2),
                new Answer("Stop them, with force", str: 2),
                new Answer("Stay out of it", vit: 2)
                ),
            new Question(
                "Do you enjoy reading books?",
                new Answer("Yes", intel: 2),
                new Answer("No", str: 2),
                new Answer("Only short ones", vit: 2)
                )
        };
        Question[] pool3 = new Question[] {
            new Question(
                "There's someone behind you!",
                new Answer("Fight them", str: 3),
                new Answer("I won't fall for that", intel: 3),
                new Answer("I'm not looking back", dex: 3)
            ),
            new Question(
                "You wake up to the sound of a crash downstairs...",
                new Answer("Confront", str: 3),
                new Answer("Call the police", vit: 3),
                new Answer("Lock your door", intel: 3),
                new Answer("Hide", dex: 3)
                ),
            new Question(
                "Do you enjoy a challenge?",
                new Answer("Yes", intel: 3, dex: 3),
                new Answer("No", str: 3, vit: 3)
                ),
            new Question(
                "You fight bravely, but you are defeated."
                + "\nYour foe is impressed with your skills and asks you to join them...",
                new Answer("Accept", vit: 3, intel: 3),
                new Answer("Refuse", str: 3),
                new Answer("Sneak attack!", dex: 3, intel: 3)
                ),
            new Question(
                "You face a pair of enemies...",
                new Answer("Fight them together", str: 3),
                new Answer("Fight them one by one", vit: 3),
                new Answer("Turn them against each other", intel: 3)
                ),
            new Question(
                "What are your thoughts on bare-handed fishing?",
                new Answer("Yes", str: 3),
                new Answer("No", intel: 3),
                new Answer("Ouch", vit: 3)
                ),
            new Question(
                "Do you enjoy doing the same thing over and over again?",
                new Answer("Yes", str: 3),
                new Answer("No", intel: 3)
                )
        };
        Question[] pool4 = new Question[] {
            new Question(
                "Are you a 1, or are you a 0?",
                new Answer("1", str: 4, vit: 2),
                new Answer("0", intel: 4, dex: 2)
                ),
            new Question(
                "What is your favorite weapon?",
                new Answer("Scimitar", dex: 4),
                new Answer("Maul", str: 4),
                new Answer("Hasta", vit: 4),
                new Answer("Fungus staff", intel: 4)
                ),
            new Question(
                "Earth or sky?",
                new Answer("Earth", vit: 4),
                new Answer("Sky", intel: 4)
                ),
            new Question(
                "Do you enjoy inventing things?",
                new Answer("Yes", intel: 4, dex: 4),
                new Answer("No", str: 4, vit: 4)
                ),
            new Question(
                "Do you ever feel as if you've lost control of your life?",
                new Answer("No", str: 4, intel: 2),
                new Answer("Yes", dex: 4, intel: 2)
                ),
            new Question(
                "Would you rather fight five weak enemies, or one strong enemy?",
                new Answer("5 Weak", intel: 4, dex: 2),
                new Answer("1 Strong", str: 4, vit: 2)
                ),
            new Question(
                "What item belongs around a cat's neck?",
                new Answer("Collar", str: 4),
                new Answer("Bandana", dex: 4),
                new Answer("Tie", intel: 4),
                new Answer("Bow", vit: 4)
                )
        };

        pool1 = pool1.Shuffle().ToArray();
        pool2 = pool2.Shuffle().ToArray();
        pool3 = pool3.Shuffle().ToArray();
        pool4 = pool4.Shuffle().ToArray();

        Quiz = new Page[] {
            QuestionToPage(pool1[0]),
            QuestionToPage(pool1[1]),
            QuestionToPage(pool2[0]),
            QuestionToPage(pool2[1]),
            QuestionToPage(pool3[0]),
            QuestionToPage(pool3[1]),
            QuestionToPage(pool4[0]),
            QuestionToPage(pool4[1])
        };

        ChooseName = new ReadPage(
            text: "What is your name?",
            hasInputField: true,
            buttonables: new Process[] {
                new Process(
                    "Confirm",
                    "Choose this name.",
                    () => {
                        Page = AttAssign;
                    },
                    () => ChooseName.InputtedString.Length >= 4)
            },
            onTick:
            () => {
                pc.Name = ChooseName.InputtedString;
            }
            );

        IDictionary<AttributeType, int> bonuses = new Dictionary<AttributeType, int>();
        AttAssign = new ReadPage(
            buttonables: new Process[] {
                new Process("Continue", "Start your adventure.",
                () => {
                    foreach(KeyValuePair<AttributeType, int> pair in bonuses) {
                        pc.AddToAttribute(pair.Key, false, pair.Value);
                        pc.AddToAttribute(pair.Key, true, pair.Value);
                        pc.AddToResource(ResourceType.HEALTH, false, pc.GetResourceCount(ResourceType.HEALTH, true));
                    }
                    Page = camp.Hub;
                    })
            },
            onEnter:
            () => {
                KeyValuePair<AttributeType, int>[] pairs = new KeyValuePair<AttributeType, int>[points.Count];
                bonuses = new Dictionary<AttributeType, int>();
                int index = 0;
                foreach (KeyValuePair<AttributeType, int> pair in points) {
                    bonuses.Add(pair.Key, 0);
                    pairs[index++] = pair;
                }
                pairs = pairs.OrderByDescending(p => p.Value).ToArray();
                if (pairs[1].Value <= 0) {
                    bonuses[pairs[0].Key] += 2;
                } else {
                    bonuses[pairs[0].Key] += 1;
                    bonuses[pairs[1].Key] += 1;
                }

                string attributeText = string.Join("\n", bonuses.Select(b => string.Format("{0}: {1} + {2}", b.Key.Name, pc.GetAttributeCount(b.Key, true), b.Value)).ToArray());

                Game.AddTextBox(
                    new TextBox(string.Format("{0}'s starting attributes are:\n{1}", pc.Name, attributeText))
                    );
                Game.AddTextBox(
                    new TextBox(string.Join("\n", AttributeType.ALL.Select(a => string.Format("{0}: {1}", a.Name, a.ShortDescription)).ToArray()))
                    );
            }
            );
    }

    private ReadPage QuestionToPage(Question q) {
        return new ReadPage(
            text: q.Text,
            musicLoc: "Bleeding_Out",
            buttonables:
                AnswersToProcesses(q.Answers)
            );
    }

    private Process[] AnswersToProcesses(Answer[] answers) {
        return answers.Select(
            a =>
                new Process(
                    a.Text,
                    "",
                    () => {
                        Util.Assert(answers.Length <= ActionGridView.TOTAL_BUTTON_COUNT,
                            string.Format("Too many buttons! {0}>{1}.",
                            answers.Length, ActionGridView.TOTAL_BUTTON_COUNT));

                        points[AttributeType.STRENGTH] += a.Str;
                        points[AttributeType.INTELLIGENCE] += a.Intel;
                        points[AttributeType.DEXTERITY] += a.Dex;
                        points[AttributeType.VITALITY] += a.Vit;

                        if (++quizIndex < Quiz.Length) {
                            Page = Quiz[quizIndex];
                        } else {
                            Page = ChooseName;
                        }
                    }
                )
            )
            .ToArray();
    }

    private struct Question {
        public readonly string Text;
        public readonly Answer[] Answers;

        public Question(string text, params Answer[] answers) {
            this.Text = text;
            this.Answers = answers.Shuffle().ToArray();
        }
    }

    private struct Answer {
        public readonly string Text;
        public readonly int Str;
        public readonly int Intel;
        public readonly int Dex;
        public readonly int Vit;

        public Answer(string text, int str = 0, int intel = 0, int dex = 0, int vit = 0) {
            this.Text = text;
            this.Str = str;
            this.Intel = intel;
            this.Dex = dex;
            this.Vit = vit;
        }
    }
}
