using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDatabase", menuName = "Scene/Database")]
public class Database : ScriptableObject
{
    public Sprite instructionImage;

    [TextAreaAttribute]
    public string instruction;

    [TextAreaAttribute]
    public string hint;

    public int gameTime;
}

// public class Data
// {
//     public Sprite instructionImage;
//     public string instruction;
//     public string hint;
//     public string level;

//     public Data(Sprite instructionImage, string instruction, string hint, string level)
//     {
//         this.instructionImage = instructionImage;
//         this.instruction = instruction;
//         this.hint = hint;
//         this.level = level;
//     }
// }

// public class Database : MonoBehaviour
// {
//     private GameManager manager;

//     [Header("Instruction image")]
//     [SerializeField] private Sprite confirmFace;
//     [SerializeField] private Sprite moveAround;
//     [SerializeField] private Sprite moveAroundWithEyeBlink;
//     [SerializeField] private Sprite headTilt;

//     public Sprite currentInstructionImage;
//     public string currentInstruction;
//     public string currentHint;
//     public string currentLevel;

//     private Data Title_Screen;
//     private Data Ghost_Avoid_Light;
//     private Data Shoot_The_Target;
//     private Data Game_Over;

//     void Start()
//     {
//         manager = gameObject.GetComponent<GameManager>();
//     }

//     void Update()
//     {
//         Title_Screen = new Data(
//             confirmFace,
//             "",
//             "",
//             "Face the camera"
//         );

//         Ghost_Avoid_Light = new Data(
//             moveAround, 
//             "鏡頭範圍內，移動臉部", 
//             "躲避光線!", 
//             "Level " + manager.countOfScenesHasLoaded
//         );

//         Shoot_The_Target = new Data(
//             moveAroundWithEyeBlink,
//             "鏡頭範圍內，移動臉部並眨眼",
//             "擊破所有\n木箱&瓶子!",
//             "Level " + manager.countOfScenesHasLoaded
//         );

//         Game_Over = new Data(
//             null,
//             "",
//             "GAME\nOVER",
//             "Result\nLevel " + manager.countOfScenesHasLoaded
//         );

//         switch (SceneManager.GetActiveScene().name)
//         {
//             case "Title_Screen":
//                 currentInstructionImage = Title_Screen.instructionImage;
//                 currentInstruction = Title_Screen.instruction;
//                 currentHint = Title_Screen.hint;
//                 currentLevel = Title_Screen.level;
//                 break;

//             case "Ghost_Avoid_Light":
//                 currentInstructionImage = Ghost_Avoid_Light.instructionImage;
//                 currentInstruction = Ghost_Avoid_Light.instruction;
//                 currentHint = Ghost_Avoid_Light.hint;
//                 currentLevel = Ghost_Avoid_Light.level;
//                 break;

//             case "Shoot_The_Target":
//                 currentInstructionImage = Shoot_The_Target.instructionImage;
//                 currentInstruction = Shoot_The_Target.instruction;
//                 currentHint = Shoot_The_Target.hint;
//                 currentLevel = Shoot_The_Target.level;
//                 break;

//             case "Game_Over":
//                 currentInstructionImage = Game_Over.instructionImage;
//                 currentInstruction = Game_Over.instruction;
//                 currentHint = Game_Over.hint;
//                 currentLevel = Game_Over.level;
//                 break;
//         }
//     }
// }
