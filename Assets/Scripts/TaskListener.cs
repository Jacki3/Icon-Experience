using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Generates random task and icon sheet which checks that the corresponding icon matches the task and takes action based on user response

public class TaskListener : MonoBehaviour
{
    string iconName;
    List<Sprite> TotalIcons = new List<Sprite>();
    List<Sprite> CustomTotalIcons = new List<Sprite>();
    List<string> taskList = new List<string>();
    List<Button> ButtonList = new List<Button>();
    Sprite randSprite;
    Sprite RandCustomSprite;
    string RandtaskName;
    int counter = 0;
    bool iconsVisible = true;
    string textTaskName;
    int taskCount;
    float timeLeft;
    float decisionTime;
    string buttonPressedName;
    int guessCount;
    float firstGuessTime;
    float secondGuessTime;
    float thirdGuessTime;
    bool isCorrect = false;
    string firstGuessIconName;
    string secondGuessIconName;
    string thirdGuessIconName;
    float currentTaskHealth;
    float totalTaskHealth = 100;
    int taskScore = 0;
    int RandButtonIndex;
    string taskCountKey = "taskCountKey";
    string scoreKey = "scoreKey";
    string playTimeKey = "playTimeKey";
    float defPlayTime;


    public float playTime;
    public string[] taskNames = new string[] { };
    public Sprite[] ToAccelerate;
    public Sprite[] ToBandage;
    public Sprite[] ToBlow;
    public Sprite[] ToBrake;
    public Sprite[] ToBrushTeeth;
    public Sprite[] ToCallfriends;
    public Sprite[] ToChargeBattery;
    public Sprite[] ToCheckOxy;
    public Sprite[] ToCheckStation;
    public Sprite[] ToCheckTemp;
    public Sprite[] ToCheckup;
    public Sprite[] ToCleanUpDust;
    public Sprite[] ToCook;
    public Sprite[] ToCut;
    public Sprite[] ToDance;
    public Sprite[] ToDig;
    public Sprite[] ToDiscoverCountrySide;
    public Sprite[] ToDoDailyReports;
    public Sprite[] ToDoGravityChecks;
    public Sprite[] ToDraw;
    public Sprite[] ToDrink;
    public Sprite[] ToDrive;
    public Sprite[] ToEat;
    public Sprite[] ToGoLeftRight;
    public Sprite[] ToGoRound;
    public Sprite[] ToHarvest;
    public Sprite[] ToHeat;
    public Sprite[] ToLearnNewLanguage;
    public Sprite[] ToLift;
    public Sprite[] ToLightUpDown;
    public Sprite[] ToListenToTheNews;
    public Sprite[] TolookForAliens;
    public Sprite[] ToPestControl;
    public Sprite[] ToPlayGames;
    public Sprite[] ToPlayMusic;
    public Sprite[] ToReadBooks;
    public Sprite[] toRockCollect;

    public Sprite[] ToScrew;
    public Sprite[] ToSleep;
    public Sprite[] ToSow;
    public Sprite[] ToTakeBath;
    public Sprite[] ToTakePictures;
    public Sprite[] ToToilet;
    public Sprite[] ToTrain;
    public Sprite[] ToTreat;
    public Sprite[] ToVacuum;
    public Sprite[] ToWashClothes;
    public Sprite[] ToWatchTV;
    public Sprite[] ToWater;
    public Sprite[] ToWearOuterWear;
    public Button[] buttonImages;
    public GameObject goodJobDisplay;
    public GameObject badJobDisplay;
    public GameObject iconIncorrectDisplay;
    public Text TaskNotification;
    public GameObject iconSheet;
    public Button startButton;
    public Text timerText;
    public Image healthBar;
    public Text playTimeText;
    public Text scoreText;
    [HideInInspector]
    public bool timerOn = false;
    public Text tasksLeftNo;

    public bool UseCustomTarget = false;
    public Sprite customTarget;
    public bool UseCustomTask = false;
    public string CustomTaskName;
    Animator idleAnimation;
    Animator completeAnimation;
    public Camera LabCam;
    public Camera mainCam;

    void Start()
    {
        defPlayTime = playTime; //Hard coding the time
        taskList = new List<string>(taskNames); //Filling the task list from the array
        textTaskName = RandtaskName;
        StartCoroutine(DelayRandomTask()); //Generate a task
        currentTaskHealth = totalTaskHealth;

        //Setting player prefs so the data is persistent 
        healthBar.transform.localScale = new Vector3(PlayerPrefs.GetFloat("TaskBar"), 1, 1);
        playTime = PlayerPrefs.GetFloat(playTimeKey);
        taskCount = PlayerPrefs.GetInt(taskCountKey);
        taskScore = PlayerPrefs.GetInt(scoreKey);
    }

    IEnumerator displayGoodJob()
    {
        goodJobDisplay.SetActive(true);
        yield return new WaitForSeconds(2);
        goodJobDisplay.SetActive(false);
    }
    IEnumerator displayBadJob()
    {
        badJobDisplay.SetActive(true);
        yield return new WaitForSeconds(2);
        badJobDisplay.SetActive(false);
    }
    IEnumerator incorrectGuessDisplay()
    {
        iconIncorrectDisplay.SetActive(true);
        yield return new WaitForSeconds(1);
        iconIncorrectDisplay.SetActive(false);
    }

    void RandomTask()
    {
        if (UseCustomTask == false) //If use random task is turned off then a random task is picked and vise versa
        {
            RandtaskName = taskList[Random.Range(0, (taskList.Count))];
        }
        else
        {
            RandtaskName = CustomTaskName;
        }

        if (taskList.Count > 1)
        {
            taskList.Remove(RandtaskName);
        }

        TaskNotification.text = ("Task to complete is: " + RandtaskName); //Outputting the task to the game

        RandomiseIcons();
    }

    IEnumerator DelayRandomTask() //Delays the method above as to avoid overlap
    {
        //Resetting task values
        timeLeft = 30.0f; //Used if timer on induvidual tasks is required
        counter = 0;
        guessCount = 0;
        decisionTime = 0.0f;
        firstGuessTime = 0.0f;
        secondGuessTime = 0.0f;
        thirdGuessTime = 0.0f;
        TaskNotification.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        RandomTask();
        TaskNotification.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
    }

    public void StartTask() //Gets called when 'start' button is pressed; turns off any task related UI and activiates the icon sheet
    {
        iconsVisible = false;
        startButton.gameObject.SetActive(false);
        TaskNotification.gameObject.SetActive(false);
        iconSheet.SetActive(true);
        timerText.gameObject.SetActive(true);
        timerOn = true;
    }

    public void RandomiseIcons() // Randomises icons based on which task is currently active
    {
        //Adding all the icons to one shared list
        TotalIcons.AddRange(ToAccelerate);
        TotalIcons.AddRange(ToBandage);
        TotalIcons.AddRange(ToBlow);
        TotalIcons.AddRange(ToBrake);
        TotalIcons.AddRange(ToBrushTeeth);
        TotalIcons.AddRange(ToCallfriends);
        TotalIcons.AddRange(ToChargeBattery);
        TotalIcons.AddRange(ToCheckOxy);
        TotalIcons.AddRange(ToCheckStation);
        TotalIcons.AddRange(ToCheckTemp);
        TotalIcons.AddRange(ToCheckup);
        TotalIcons.AddRange(ToCleanUpDust);
        TotalIcons.AddRange(ToCook);
        TotalIcons.AddRange(ToCut);
        TotalIcons.AddRange(ToDance);
        TotalIcons.AddRange(ToDig);
        TotalIcons.AddRange(ToDiscoverCountrySide);
        TotalIcons.AddRange(ToDoDailyReports);
        TotalIcons.AddRange(ToDoGravityChecks);
        TotalIcons.AddRange(ToDraw);
        TotalIcons.AddRange(ToDrink);
        TotalIcons.AddRange(ToDrive);
        TotalIcons.AddRange(ToEat);
        TotalIcons.AddRange(ToGoLeftRight);
        TotalIcons.AddRange(ToGoRound);
        TotalIcons.AddRange(ToHarvest);
        TotalIcons.AddRange(ToLearnNewLanguage);
        TotalIcons.AddRange(ToLift);
        TotalIcons.AddRange(ToLightUpDown);
        TotalIcons.AddRange(ToListenToTheNews);
        TotalIcons.AddRange(TolookForAliens);
        TotalIcons.AddRange(ToPestControl);
        TotalIcons.AddRange(ToPlayGames);
        TotalIcons.AddRange(ToPlayMusic);
        TotalIcons.AddRange(ToReadBooks);
        TotalIcons.AddRange(ToScrew);
        TotalIcons.AddRange(ToSleep);
        TotalIcons.AddRange(ToSow);
        TotalIcons.AddRange(ToTakeBath);
        TotalIcons.AddRange(ToTakePictures);
        TotalIcons.AddRange(ToToilet);
        TotalIcons.AddRange(ToTrain);
        TotalIcons.AddRange(ToTreat);
        TotalIcons.AddRange(ToVacuum);
        TotalIcons.AddRange(ToWashClothes);
        TotalIcons.AddRange(ToWatchTV);
        TotalIcons.AddRange(ToWater);
        TotalIcons.AddRange(ToHeat);
        TotalIcons.AddRange(toRockCollect);
        TotalIcons.AddRange(ToWearOuterWear);

        ButtonList.AddRange(buttonImages);

        switch (RandtaskName)
        {
            case "Before cooking, you need to heat that hotplate!":
                {
                    SortIcons(ToHeat); //See method below; each task must have a corresponding icon list
                }
                break;
            case "It seems that this room need some air!":
                {
                    SortIcons(ToBlow);
                }
                break;
            case "Let's cook some pasta!":
                {
                    SortIcons(ToCook);
                }
                break;
            case "Don't forget to water your harvest!":
                {
                    SortIcons(ToWater);
                }
                break;
            case "If you want to eat, first you need to sow!":
                {
                    SortIcons(ToSow);
                }
                break;
            case "It's time to harvest!":
                {
                    SortIcons(ToHarvest);
                }
                break;
            case "Is the level of oxygen ok?":
                {
                    SortIcons(ToCheckOxy);
                }
                break;
            case "It's hot in here, are you sure the heating system is ok?":
                {
                    SortIcons(ToCheckTemp);
                }
                break;
            case "It's time for your weekly vitamines shots!":
                {
                    SortIcons(ToTreat);
                }
                break;
            case "This wound doesn't look good, put a bandage on it.":
                {
                    SortIcons(ToBandage);
                }
                break;
            case "Weekly sport session ahead!":
                SortIcons(ToTrain);
                break;
            case "Everything is undercontrol, time for bed.":
                SortIcons(ToSleep);
                break;
            case "Don't forget to hydrate!":
                SortIcons(ToDrink);
                break;
            case "Yummy, time to eat!":
                SortIcons(ToEat);
                break;
            case "It seems that you need to go to the toilet!":
                SortIcons(ToToilet);
                break;
            case "Even astronaut have to brush their theeths!":
                SortIcons(ToBrushTeeth);
                break;
            case "It's time to take a bath!":
                SortIcons(ToTakeBath);
                break;
            case "We are running out of power, hurry up, charge the battery?":
                StartCoroutine(PlayIdleTaskAnim("ToChargeBatteryIdle", LabCam)); //An example of how to play animation which is task dependent, call the method below and give an animation and room 
                SortIcons(ToChargeBattery);
                break;
            case "Weekly housework please!":
                SortIcons(ToVacuum);
                break;
            case "Dust on the floor, where is the broom?":
                SortIcons(ToCleanUpDust);
                break;
            case "Can you fix this shelf?":
                SortIcons(ToScrew);
                break;
            case "Dig that hole!":
                SortIcons(ToDig);
                break;
            case "Light up!":
                SortIcons(ToLightUpDown);
                break;
            case "You don't have anything to wear! Launch a washing machine.":
                SortIcons(ToWashClothes);
                break;
            case "It's time to go out there, put on your outerwear.":
                SortIcons(ToWearOuterWear);
                break;
            case "Hey grandma, can you go faster?":
                SortIcons(ToAccelerate);
                break;
            case "Warning, there is a hole in front of you!":
                SortIcons(ToGoRound);
                break;
            case "Take this rover for a ride!":
                SortIcons(ToDrive);
                break;
            case "Go this way.":
                SortIcons(ToGoLeftRight);
                break;
            case "You are going to fast.Please decelerate.":
                SortIcons(ToBrake);
                break;
            case "We don't want any parasite, pest control!":
                SortIcons(ToPestControl);
                break;
            case "Put that up there.":
                SortIcons(ToLift);
                break;
            case "Can you cut that in half?":
                SortIcons(ToCut);
                break;
            case "Let's play some music.":
                SortIcons(ToPlayMusic);
                break;
            case "Move that hips, dance!":
                SortIcons(ToDance);
                break;
            case "There is no friend as loyal as a book, it's time to read.":
                SortIcons(ToReadBooks);
                break;
            case "Don't you miss your friends? Call them to know what's up.":
                SortIcons(ToCallfriends);
                break;
            case "It's important to be up to date with what is going on in earth.":
                SortIcons(ToListenToTheNews);
                break;
            case "Enough work, it's time to play and have some fun!":
                SortIcons(ToPlayGames);
                break;
            case "It's never too late to learn a new language.":
                SortIcons(ToLearnNewLanguage);
                break;
            case "Not all who wander are lost, go for a walk!":
                SortIcons(ToDiscoverCountrySide);
                break;
            case "Let your inner Picasso express himself, draw something.":
                SortIcons(ToDraw);
                break;
            case "Let's watch some telly.":
                SortIcons(ToWatchTV);
                break;
            case "Time to write a report for the station.":
                SortIcons(ToDoDailyReports);
                break;
            case "Work in progress, let's do some gravity tests.":
                SortIcons(ToDoGravityChecks);
                break;
            case "It's time for the weekly medical check up.":
                SortIcons(ToCheckup);
                break;
            case "What a beautiful a scenery, take a picture!":
                SortIcons(ToTakePictures);
                break;
            case "Could you check if everything is fine with the station?":
                SortIcons(ToCheckStation);
                break;
            case "Do you think there is some aliens on this planet? Let's found out.":
                SortIcons(TolookForAliens);
                break;
            case "We must take back some rock on earth in order to analyse them.":
                SortIcons(toRockCollect);
                break;
        }
    }

    public void SortIcons(Sprite[] iconType) //Sorts icons based on which icon list has been given (what task has been called)
    {
        CustomTotalIcons.AddRange(TotalIcons.Except(iconType).ToList()); //Adding all the icons beside from the one which is related to the task

        RandButtonIndex = Random.Range(0, (ButtonList.Count)); //Getting a random icon placeholder from the iconsheet
        if (UseCustomTarget == false)
        {
            RandCustomSprite = iconType[Random.Range(0, iconType.Length)]; //If not using custom image, choose a random image from the given list
        }
        else
        {
            RandCustomSprite = customTarget;
        }

        ButtonList[RandButtonIndex].image.sprite = RandCustomSprite;
        ButtonList[RandButtonIndex].name = ButtonList[RandButtonIndex].image.sprite.name; //Assigning the image to the placeholder and giving it a name

        for (int i = 0; i < ButtonList.Count; i++) //For the icon placeholders that are left; fill them in with random icon images from the shared list
        {
            if ((i != RandButtonIndex))
            {
                randSprite = CustomTotalIcons[Random.Range(0, CustomTotalIcons.Count)];
                ButtonList[i].image.sprite = randSprite;
                ButtonList[i].name = ButtonList[i].image.sprite.name;
            }
        }
        ButtonList.Clear();
        CustomTotalIcons.Clear();
    }

    IEnumerator PlayIdleTaskAnim(string animName, Camera roomCam) //For playing animations when a task is called; give the name of the animation you wish to play and the room in which it will take place 
    {
        mainCam.gameObject.SetActive(false);
        roomCam.gameObject.SetActive(true);
        idleAnimation = GameObject.Find(animName).GetComponent<Animator>();
        idleAnimation.enabled = true;
        yield return new WaitForSeconds(3);
        roomCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
    }

    public void IconDetection()
    {
        iconName = (EventSystem.current.currentSelectedGameObject.name); //This is getting the name of the icon pressed
        checkisCorrect(iconName); //See method below for description
    }

    void checkisCorrect(string iconName) //This is going through each task; there is a corresponding icon name which when pressed will complete the task and if not will add an incorrect guess count
    {

        switch (RandtaskName)
        {
            case "Before cooking, you need to heat that hotplate!":
                if (iconName.Contains("toheat"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++; //Counting how many incorrect guesses
                    StartCoroutine(incorrectGuessDisplay()); //Will show the 'NOPE' when an incorrect guess is made
                }
                break;
            case "It seems that this room need some air!":
                if (iconName.Contains("toblow"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Let's cook some pasta!":
                if (iconName.Contains("tocook"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Don't forget to water your harvest!":
                if (iconName.Contains("towater"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "If you want to eat, first you need to sow!":
                if (iconName.Contains("tosow"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "It's time to harvest!":
                if (iconName.Contains("toharvest"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Is the level of oxygene ok?":
                if (iconName.Contains("tocheckoxy"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "It's hot in here, are you sure the heating system is ok?":
                if (iconName.Contains("tochecktemp"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "It's time for your weekly vitamines shots!":
                if (iconName.Contains("totreat"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "This wound doesn't look good, put a bandage on it.":
                if (iconName.Contains("tobandage"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Weekly sport session ahead!":
                if (iconName.Contains("totrain"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Everything is undercontrol, time for bed.":
                if (iconName.Contains("tosleep"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Don't forget to hydrate!":
                if (iconName.Contains("todrink"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Yummy, time to eat!":
                if (iconName.Contains("toeat"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "It seems that you need to go to the toilet!":
                if (iconName.Contains("totoilet"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Even astronaut have to brush their theeths!":
                if (iconName.Contains("tobrushtheeth"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "It's time to take a bath!":
                if (iconName.Contains("totakeabath"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "We are running out of power, hurry up, charge the battery?":
                if (iconName.Contains("tochargebattery"))
                {
                    isCorrect = true;
                    StartCoroutine(PlayCompletedTaskAnim("ToChargeBatteryComplete", LabCam)); //Plays the correctly guessed animation (see method below)
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Weekly housework please!":
                if (iconName.Contains("tovacuum"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Dust on the floor, where is the broom?":
                if (iconName.Contains("tocleanupdust"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Can you fix this shelf?":
                if (iconName.Contains("toscrew"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Dig that hole!":
                if (iconName.Contains("todig"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Light up!":
                if (iconName.Contains("tolightupdown"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "You don't have anything to wear! Launch a washing machine.":
                if (iconName.Contains("towasclothes"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "It's time to go out there, put on your outerwear.":
                if (iconName.Contains("towearouterwear"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Hey grandma, can you go faster?":
                if (iconName.Contains("toaccelerate"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Warning, there is a hole in front of you!":
                if (iconName.Contains("togoround"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Take this rover for a ride!":
                if (iconName.Contains("todrive"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Go this way.":
                if (iconName.Contains("togoleftright"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "You are going to fast. Please decelerate.":
                if (iconName.Contains("tobrake"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "We don't want any parasite, pest control!":
                if (iconName.Contains("topestcontrol"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Put that up there.":
                if (iconName.Contains("tolift"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Can you cut that in half?":
                if (iconName.Contains("tocut"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Let's play some music.":
                if (iconName.Contains("toplaymusic"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Move that hips, dance!":
                if (iconName.Contains("todance"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "There is no friend as loyal as a book, it's time to read.":
                if (iconName.Contains("toreadbooks"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Don't you miss your friends? Call them to know what's up.":
                if (iconName.Contains("tocallfriends"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "It's important to be up to date with what is going on in earth.":
                if (iconName.Contains("tolistentothenews"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Enough work, it's time to play and have some fun!":
                if (iconName.Contains("toplaygames"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "It's never too late to learn a new language.":
                if (iconName.Contains("tolearnnewlanguage"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Not all who wander are lost, go for a walk!":
                if (iconName.Contains("todiscovercountryside"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Let your inner Picasso express himself, draw something.":
                if (iconName.Contains("todraw"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Let's watch some telly.":
                if (iconName.Contains("towatchtv"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Time to write a report for the station.":
                if (iconName.Contains("tododailyreports"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Work in progress, let's do some gravity tests.":
                if (iconName.Contains("todogravitychecks"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "It's time for the weekly medical check up.":
                if (iconName.Contains("tocheckup"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "What a beautiful a scenery, take a picture!":
                if (iconName.Contains("totakepictures"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Could you check if everything is fine with the station?":
                if (iconName.Contains("tocheckstation"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "Do you think there is some aliens on this planet? Let's found out.":
                if (iconName.Contains("tolookforaliens"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
            case "We must take back some rock on earth in order to analyse them.":
                if (iconName.Contains("torockcollect"))
                {
                    isCorrect = true;
                }
                else
                {
                    counter++;
                    StartCoroutine(incorrectGuessDisplay());
                }
                break;
        }
    }

    IEnumerator PlayCompletedTaskAnim(string animName, Camera roomCam) //An animation which plays if correctly guessed (called when the user guesses correctly)
    {
        mainCam.gameObject.SetActive(false);
        roomCam.gameObject.SetActive(true); //Switching cameras
        idleAnimation.gameObject.SetActive(false);
        completeAnimation = GameObject.Find(animName).GetComponent<Animator>();
        completeAnimation.enabled = true; //Playing the animation 
        yield return new WaitForSeconds(5);
        roomCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
    }

    public void setTimeonClick() // Used for calculating decision time and name of icon pressed
    {
        guessCount++;
        buttonPressedName = EventSystem.current.currentSelectedGameObject.name;
    }

    IEnumerator DelayLevelLoad() //Loads the next scene (called when timer is complete)
    {
        yield return new WaitForSeconds(0);
        PlayerPrefs.SetFloat(playTimeKey, playTime = defPlayTime);
        PlayerPrefs.SetInt(taskCountKey, taskCount = 0);
        SceneManager.LoadScene("Survey");
    }

    public void Update()
    {
        PlayerPrefs.GetFloat(playTimeKey);

        playTime -= Time.deltaTime; // Start the timer
        int minutes = Mathf.FloorToInt(playTime / 60F);
        int seconds = Mathf.FloorToInt(playTime - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        playTimeText.text = (niceTime); //Converted time so it is nicer to read

        if (timerOn == true)
        {
            // For Setting timer on induvidual tasks

            // timeLeft -= Time.deltaTime;
            // int minutesT = Mathf.FloorToInt(timeLeft / 60F);
            // int secondsT = Mathf.FloorToInt(timeLeft - minutesT * 60);
            // string niceTimeT = string.Format("{0:0}:{1:00}", minutesT, secondsT);
            // timerText.text = (niceTimeT);

            // if (timeLeft < 0)
            // {
            //     counter = 3;
            // }

            //Running a timer when a task starts in order to determine the decision time
            decisionTime += Time.deltaTime;
        }

        textTaskName = RandtaskName;
        int tasksLeft = 10 - taskCount;
        tasksLeftNo.text = (tasksLeft.ToString());

        //Getting the decision time on each guess made 
        if (guessCount == 1)
        {
            firstGuessTime = decisionTime;
            firstGuessIconName = buttonPressedName;
        }
        if (guessCount == 2)
        {
            secondGuessTime = decisionTime - firstGuessTime;
            secondGuessIconName = buttonPressedName;
        }
        if (guessCount == 3)
        {
            thirdGuessTime = decisionTime - secondGuessTime;
            thirdGuessIconName = buttonPressedName;
        }

        if (counter == 3) //Determining score based on guesses made and if it is 3 then the code below will activate 
        {
            if (guessCount == 1)
            {
                taskScore = taskScore + 15;
            }
            else if (guessCount == 2)
            {
                taskScore = taskScore + 10;
            }
            else if (guessCount == 3 && counter != 3)
            {
                taskScore = taskScore + 5;
            }

            //Use when all animations have been set

            // if (idleAnimation.enabled == true)
            // {
            //     idleAnimation.enabled = false;
            // }

            StartCoroutine(displayBadJob()); //Show the user it is incorrect
            iconSheet.SetActive(false); //Turn off icon sheet
            TaskNotification.gameObject.SetActive(true); //Show the next task
            startButton.gameObject.SetActive(true); //Show the start button again
            iconsVisible = true;
            CreateFile(); //Create a file in which the decision time and guesses made info will be displayed
            timerOn = false;
            timerText.gameObject.SetActive(false);
            taskCount++; //Add to task count
            isCorrect = false;
            StartCoroutine(DelayRandomTask()); //Generate a new task
        }

        if (isCorrect == true) //If user guesses correctly (once again score is measured here)
        {
            if (guessCount == 1)
            {
                taskScore = taskScore + 15;
            }
            else if (guessCount == 2)
            {
                taskScore = taskScore + 10;
            }
            else if (guessCount == 3 && counter != 3)
            {
                taskScore = taskScore + 5;
            }

            StartCoroutine(displayGoodJob());
            iconSheet.SetActive(false);
            TaskNotification.gameObject.SetActive(true);
            startButton.gameObject.SetActive(true);
            iconsVisible = true;
            CreateFile();
            timerOn = false;
            timerText.gameObject.SetActive(false);
            taskCount++;
            isCorrect = false;
            StartCoroutine(DelayRandomTask());
        }

        scoreText.text = "Score: " + taskScore.ToString(); //Outputting score to screen 

        if (taskCount == 10) // If all task for the day have been completed (change this to allow for more task to be carried out in one day)
        {
            TaskNotification.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
        }

        if (playTime < 0) //If the time for the day is up then load the next scene 
        {
            StartCoroutine(DelayLevelLoad());
        }

        //Setting the values of score etc so it persists outside of game
        PlayerPrefs.SetInt(scoreKey, taskScore);
        PlayerPrefs.SetInt(taskCountKey, taskCount);
        PlayerPrefs.SetFloat(playTimeKey, playTime);
    }

    void CreateFile()
    {
        var safeTaskName = textTaskName.Contains("?") ? textTaskName.Replace("?", "") : textTaskName; //No question marks in text file name 

        if (File.Exists(safeTaskName + ".txt")) //Stops overwriting of overlapping tasks
        {
            Debug.Log("Task already completed!");
        }
        else
        {
            string directoryPath = Application.dataPath + "/Icons"; //Change this if need be (Will go to the /assets folder automatically)

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Application.dataPath + "/Icons");
            }

            //Writing the lines in the text file (decision time for each guess and what that guess was)

            string filePath = Application.dataPath + "/Icons/" + safeTaskName + ".txt";
            var sr = File.CreateText(filePath);
            sr.WriteLine("Total Guesses Made: " + guessCount);
            sr.WriteLine("Incorrect Guesses: " + counter);
            if (counter == 3)
                sr.WriteLine("TASK FAILED");
            sr.WriteLine(" ");
            float totalTimeTaken = firstGuessTime + secondGuessTime + thirdGuessTime;
            sr.WriteLine("Total Time Taken: " + totalTimeTaken);
            sr.WriteLine(" ");
            sr.WriteLine("First Guess:");
            sr.WriteLine("Time taken to choose: " + firstGuessTime);
            sr.WriteLine("Icon attempted: " + firstGuessIconName);
            sr.WriteLine(" ");
            sr.WriteLine("Second Guess:");
            sr.WriteLine("Time taken to choose: " + secondGuessTime);
            sr.WriteLine("Icon attempted: " + secondGuessIconName);
            sr.WriteLine(" ");
            sr.WriteLine("Third Guess:");
            sr.WriteLine("Time taken to choose: " + thirdGuessTime);
            sr.WriteLine("Icon attempted: " + thirdGuessIconName);
            sr.Close();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}