using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpHandlerAttribute : MonoBehaviour
{

    // LevelUpHandlerAttribute handles a level up attribute when levelling up


    #region VARIABLES


    public LevelUpAspect levelUpAspect { get; private set; }

    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private GameObject textAreaObj;
    [SerializeField] private TMP_Text textAreaText;
    [SerializeField] private TMP_Dropdown dropdownArea;
    [SerializeField] private GameObject artAndSpecAreaObj;
    [SerializeField] private ChosenLevelUpArt chosenLevelUpArtPrefab;
    [SerializeField] private LevelUpChooseSpec levelUpChooseSpecPrefab;

    [SerializeField] private GameObject initialSetupArea;
    [SerializeField] private TMP_Text[] skillScoreTexts = new TMP_Text[5];
    private int[] incSkillNums = new int[5];
    private int totalIncSkill = 40;

    private ArtData chosenArtData;
    private SpecializationData chosenSpecData;
    private bool executed = false;


    #endregion


    #region SETUP


    // Sets up attribute
    //----------------------------------------//
    public void SetupAttribute(LevelUpAspect _levelUpAspect)
    //----------------------------------------//
    {
        levelUpAspect = _levelUpAspect;
        DisplayAttribute();

    } // END SetupAttribute


    #endregion


    #region DISPLAY


    // Displays attribute
    //----------------------------------------//
    private void DisplayAttribute()
    //----------------------------------------//
    {
        List<TMP_Dropdown.OptionData> optionsList = new List<TMP_Dropdown.OptionData>();

        dropdownArea.gameObject.SetActive(false);
        textAreaObj.SetActive(false);
        artAndSpecAreaObj.SetActive(false);

        switch (levelUpAspect.levelUpType)
        {
            case LevelUpAspect.LevelUpType.SkillIncrease:
                switch(levelUpAspect.assocIncNumber)
                {
                    case -1:
                        descriptionText.text = "+1 to any Skill";
                        dropdownArea.gameObject.SetActive(true);
                        dropdownArea.ClearOptions();
                        optionsList.Add(new TMP_Dropdown.OptionData("Strength"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Dexterity"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Intelligence"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Senses"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Charisma"));
                        dropdownArea.AddOptions(optionsList);
                        dropdownArea.value = 0;
                        break;
                    case 0:
                        descriptionText.text = "+1 to Strength Skill";
                        break;
                    case 1:
                        descriptionText.text = "+1 to Dexterity Skill";
                        break;
                    case 2:
                        descriptionText.text = "+1 to Intelligence Skill";
                        break;
                    case 3:
                        descriptionText.text = "+1 to Senses Skill";
                        break;
                    case 4:
                        descriptionText.text = "+1 to Charisma Skill";
                        break;
                }
                break;
            case LevelUpAspect.LevelUpType.AppBonus:
                switch(levelUpAspect.assocString)
                {
                    case "Any":
                        descriptionText.text = "+1 Application Bonus to any Application";
                        dropdownArea.gameObject.SetActive(true);
                        dropdownArea.ClearOptions();
                        optionsList.Add(new TMP_Dropdown.OptionData("Force"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Smash"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Throw"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Wrestle"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Draw"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Jump"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Run"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Sneak"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Analyze"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Deduce"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Know"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Learn"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Feel"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Hear"));
                        optionsList.Add(new TMP_Dropdown.OptionData("React"));
                        optionsList.Add(new TMP_Dropdown.OptionData("See"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Charm"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Glean"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Manipulate"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Persuade"));
                        dropdownArea.AddOptions(optionsList);
                        dropdownArea.value = 0;
                        break;
                    case "Strength":
                        descriptionText.text = "+1 Strength Application Bonus";
                        dropdownArea.gameObject.SetActive(true);
                        dropdownArea.ClearOptions();
                        optionsList.Add(new TMP_Dropdown.OptionData("Force"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Smash"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Throw"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Wrestle"));
                        dropdownArea.AddOptions(optionsList);
                        dropdownArea.value = 0;
                        break;
                    case "Dexterity":
                        descriptionText.text = "+1 Dexterity Application Bonus";
                        dropdownArea.gameObject.SetActive(true);
                        dropdownArea.ClearOptions();
                        optionsList.Add(new TMP_Dropdown.OptionData("Draw"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Jump"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Run"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Sneak"));
                        dropdownArea.AddOptions(optionsList);
                        dropdownArea.value = 0;
                        break;
                    case "Intelligence":
                        descriptionText.text = "+1 Intelligence Application Bonus";
                        dropdownArea.gameObject.SetActive(true);
                        dropdownArea.ClearOptions();
                        optionsList.Add(new TMP_Dropdown.OptionData("Analyze"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Deduce"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Know"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Learn"));
                        dropdownArea.AddOptions(optionsList);
                        dropdownArea.value = 0;
                        break;
                    case "Senses":
                        descriptionText.text = "+1 Senses Application Bonus";
                        dropdownArea.gameObject.SetActive(true);
                        dropdownArea.ClearOptions();
                        optionsList.Add(new TMP_Dropdown.OptionData("Feel"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Hear"));
                        optionsList.Add(new TMP_Dropdown.OptionData("React"));
                        optionsList.Add(new TMP_Dropdown.OptionData("See"));
                        dropdownArea.AddOptions(optionsList);
                        dropdownArea.value = 0;
                        break;
                    case "Charisma":
                        descriptionText.text = "+1 Charisma Application Bonus";
                        dropdownArea.gameObject.SetActive(true);
                        dropdownArea.ClearOptions();
                        optionsList.Add(new TMP_Dropdown.OptionData("Charm"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Glean"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Manipulate"));
                        optionsList.Add(new TMP_Dropdown.OptionData("Persuade"));
                        dropdownArea.AddOptions(optionsList);
                        dropdownArea.value = 0;
                        break;
                    default:
                        descriptionText.text = "+1 " + levelUpAspect.assocString + " Application Bonus";
                        break;
                }
                break;
            case LevelUpAspect.LevelUpType.MaxComposureIncrease:
                descriptionText.text = "+" + levelUpAspect.assocIncNumber + "" + levelUpAspect.assocDie.ToString() + " Maximum Composure";
                break;
            case LevelUpAspect.LevelUpType.SpeedIncrease:
                descriptionText.text = "+" + levelUpAspect.assocIncNumber + " Movement Speed";
                break;
            case LevelUpAspect.LevelUpType.ThresholdIncrease:
                descriptionText.text = "+" + levelUpAspect.assocIncNumber + " Composure Threshold";
                break;
            case LevelUpAspect.LevelUpType.ActIncrease:
                descriptionText.text = "+1 maximum Act";
                break;
            case LevelUpAspect.LevelUpType.DefianceIncrease:
                descriptionText.text = "+1 maximum Defiance";
                break;
            case LevelUpAspect.LevelUpType.LearnSpecificArt:
                string complexity = "";

                if (levelUpAspect.assocIncNumber == 1)
                {
                    complexity = "Advanced ";
                }
                else if (levelUpAspect.assocIncNumber == 2)
                {
                    complexity = "Expert ";
                }

                descriptionText.text = "Learn the \"" + levelUpAspect.assocString + "\" " + complexity + levelUpAspect.assocArtType.ToString() + " Art";

                artAndSpecAreaObj.gameObject.SetActive(true);
                ArtsManager.Instance.DisplaySpecificArt(levelUpAspect.assocString, artAndSpecAreaObj.transform);

                break;
            case LevelUpAspect.LevelUpType.LearnArtFromList:
                descriptionText.text = "Learn a \"" + levelUpAspect.assocString + "\" Art";

                artAndSpecAreaObj.gameObject.SetActive(true);
                break;
            case LevelUpAspect.LevelUpType.LearnArtOfType:
                artAndSpecAreaObj.gameObject.SetActive(true);

                string complexity1 = "";

                if (levelUpAspect.assocIncNumber == 1)
                {
                    complexity1 = "Advanced ";
                }
                else if (levelUpAspect.assocIncNumber == 2)
                {
                    complexity1 = "Expert ";
                }

                descriptionText.text = "Learn any " + complexity1 + levelUpAspect.assocArtType.ToString() + " Art";
                break;
            case LevelUpAspect.LevelUpType.LearnAnyArt:
                artAndSpecAreaObj.gameObject.SetActive(true);

                string complexity2 = "";

                if (levelUpAspect.assocIncNumber == 1)
                {
                    complexity2 = "Advanced";
                }
                else if (levelUpAspect.assocIncNumber == 2)
                {
                    complexity2 = "Expert";
                }

                descriptionText.text = "Learn any " + complexity2 + " Art";
                break;
            case LevelUpAspect.LevelUpType.UnlockSpec:
                artAndSpecAreaObj.gameObject.SetActive(true);
                if (levelUpAspect.assocString == "Any" || levelUpAspect.assocString == "")
                {
                    descriptionText.text = "Unlock access to any " + (levelUpAspect.assocIncNumber + 1) + "-Star Specialization";
                }
                else
                {
                    descriptionText.text = "Unlock access to any " + (levelUpAspect.assocIncNumber + 1) + "-Star Specialization from list";
                }
                break;
            case LevelUpAspect.LevelUpType.Other:
                descriptionText.text = levelUpAspect.assocString;
                break;
            case LevelUpAspect.LevelUpType.InitialSetup:
                initialSetupArea.gameObject.SetActive(true);
                descriptionText.text = "Skill setup (allot 40 points to skills, none below 4 or above 14)";
                incSkillNums[0] = 8;
                incSkillNums[1] = 8;
                incSkillNums[2] = 8;
                incSkillNums[3] = 8;
                incSkillNums[4] = 8;
                RedisplaySetupScores();
                break;
        }

    } // END DisplayAttribute.cs


    #endregion


    #region EXCECUTE


    // Excecutes levelup
    //----------------------------------------//
    public void ExcecuteLevelup()
    //----------------------------------------//
    {
        if (executed)
        {
            return;
        }

        executed = true;

        switch (levelUpAspect.levelUpType)
        {
            case LevelUpAspect.LevelUpType.SkillIncrease:
                switch (levelUpAspect.assocIncNumber)
                {
                    case -1:
                        SkillsManager.Instance.AddToSkill(dropdownArea.options[dropdownArea.value].text, 1);
                        break;
                    case 0:
                        SkillsManager.Instance.AddToSkill("Strength", 1);
                        break;
                    case 1:
                        SkillsManager.Instance.AddToSkill("Dexterity", 1);
                        break;
                    case 2:
                        SkillsManager.Instance.AddToSkill("Intelligence", 1);
                        break;
                    case 3:
                        SkillsManager.Instance.AddToSkill("Senses", 1);
                        break;
                    case 4:
                        SkillsManager.Instance.AddToSkill("Charisma", 1);
                        break;
                }
                break;
            case LevelUpAspect.LevelUpType.AppBonus:
                switch (levelUpAspect.assocString)
                {
                    case "Any":
                    case "Strength":
                    case "Dexterity":
                    case "Intelligence":
                    case "Senses":
                    case "Charisma":
                        SkillsManager.Instance.AddToApp(dropdownArea.options[dropdownArea.value].text, 1);
                        break;
                    default:
                        SkillsManager.Instance.AddToApp(levelUpAspect.assocString, 1);
                        break;
                }
                break;
            case LevelUpAspect.LevelUpType.MaxComposureIncrease:
                int rand = 0;
                switch(levelUpAspect.assocDie)
                {
                    case InvItemsManager.DamageDie.d4:
                        rand = Random.Range(1, 4);
                        break;
                    case InvItemsManager.DamageDie.d6:
                        rand = Random.Range(1, 6);
                        break;
                    case InvItemsManager.DamageDie.d8:
                        rand = Random.Range(1, 8);
                        break;
                    case InvItemsManager.DamageDie.d10:
                        rand = Random.Range(1, 10);
                        break;
                    case InvItemsManager.DamageDie.d12:
                        rand = Random.Range(1, 12);
                        break;
                    case InvItemsManager.DamageDie.d20:
                        rand = Random.Range(1, 20);
                        break;
                }
                GeneralAreaManager.Instance.AddToMaxComposure(levelUpAspect.assocIncNumber * rand);
                break;
            case LevelUpAspect.LevelUpType.SpeedIncrease:
                GeneralAreaManager.Instance.AddToSpeedBonus(levelUpAspect.assocIncNumber);
                break;
            case LevelUpAspect.LevelUpType.ThresholdIncrease:
                GeneralAreaManager.Instance.AddToThreshold(levelUpAspect.assocIncNumber);
                break;
            case LevelUpAspect.LevelUpType.ActIncrease:
                GeneralAreaManager.Instance.AddToMaxActs(1);
                break;
            case LevelUpAspect.LevelUpType.DefianceIncrease:
                GeneralAreaManager.Instance.AddToDefiances(1);
                break;
            case LevelUpAspect.LevelUpType.LearnSpecificArt:
                ArtsManager.Instance.LearnArt(levelUpAspect.assocString);
                break;
            case LevelUpAspect.LevelUpType.LearnArtFromList:
                if (chosenArtData != null)
                    ArtsManager.Instance.LearnArt(chosenArtData);
                break;
            case LevelUpAspect.LevelUpType.LearnArtOfType:
                if (chosenArtData != null)
                    ArtsManager.Instance.LearnArt(chosenArtData);
                break;
            case LevelUpAspect.LevelUpType.LearnAnyArt:
                if (chosenArtData != null)
                    ArtsManager.Instance.LearnArt(chosenArtData);
                break;
            case LevelUpAspect.LevelUpType.UnlockSpec:
                if (chosenSpecData != null)
                    SpecializeManager.Instance.AddCharSpec(chosenSpecData);
                break;
            case LevelUpAspect.LevelUpType.Other:
                break;
            case LevelUpAspect.LevelUpType.InitialSetup:
                SkillsManager.Instance.AddToSkill("Strength", incSkillNums[0]);
                SkillsManager.Instance.AddToSkill("Dexterity", incSkillNums[1]);
                SkillsManager.Instance.AddToSkill("Intelligence", incSkillNums[2]);
                SkillsManager.Instance.AddToSkill("Senses", incSkillNums[3]);
                SkillsManager.Instance.AddToSkill("Charisma", incSkillNums[4]);
                break;
        }

    } // END ExcecuteLevelup


    #endregion


    #region ON CHOOSE ART / SPEC


    // Begins choosing art
    //----------------------------------------//
    public void OnBeginChooseArt()
    //----------------------------------------//
    {
        FindObjectOfType<LevelUpHandler>().OpenChooseArtPanel(this);

    } // END OnBeginChooseArt


    // Chooses art and places in slot
    //----------------------------------------//
    public void OnChooseArt(ArtData _chosenArtData)
    //----------------------------------------//
    {
        chosenArtData = _chosenArtData;

        foreach (Transform child in artAndSpecAreaObj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        ChosenLevelUpArt newChosenArt = GameObject.Instantiate(chosenLevelUpArtPrefab);
        newChosenArt.transform.SetParent(artAndSpecAreaObj.transform);
        newChosenArt.SetupArt(chosenArtData);
        newChosenArt.SetAttribute(this);

    } // END OnChooseArt


    // Chooses spec and places in slot
    //----------------------------------------//
    public void OnChooseSpec(SpecializationData _chosenSpecData)
    //----------------------------------------//
    {
        chosenSpecData = _chosenSpecData;

        foreach (Transform child in artAndSpecAreaObj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        LevelUpChooseSpec newChosenSpec = GameObject.Instantiate(levelUpChooseSpecPrefab);
        newChosenSpec.transform.SetParent(artAndSpecAreaObj.transform);
        newChosenSpec.SetupSpec(chosenSpecData, this, "Change");

    } // END OnChooseArt


    #endregion


    #region INITIAL SKILL SETUP


    // Redisplays setup scores from data
    //----------------------------------------//
    public void RedisplaySetupScores()
    //----------------------------------------//
    {
        for (int i = 0; i < 5; i++)
        {
            skillScoreTexts[i].text = incSkillNums[i].ToString();
        }

    } // END RedisplaySetupScores


    // Attempts to increase a skill
    //----------------------------------------//
    public void OnInitialSetupSkillUp(int skill)
    //----------------------------------------//
    {
        if (totalIncSkill < 40 && incSkillNums[skill] < 14)
        {
            incSkillNums[skill] += 1;
            totalIncSkill++;
            RedisplaySetupScores();
        }

    } // END OnInitialSetupSkillUp


    // Attempts to decrease a skill
    //----------------------------------------//
    public void OnInitialSetupSkillDown(int skill)
    //----------------------------------------//
    {
        if (incSkillNums[skill] > 4)
        {
            incSkillNums[skill] -= 1;
            totalIncSkill--;
            RedisplaySetupScores();
        }

    } // END OnInitialSetupSkillDown


    #endregion


} // END LevelUpHandlerAttribute.cs
