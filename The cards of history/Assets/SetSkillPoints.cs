using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkillPoints : MonoBehaviour
{
    public CharacterMakerManager chman;

    public void SetDmgBuff(bool added)
    {

        if(added && chman.copyChar.skillPoints > 0)
        {
            chman.copyChar.dmg_buff++;
            chman.copyChar.skillPoints--;
        }          
        else if (!added && chman.copyChar.dmg_buff > 0)
        {
            chman.copyChar.dmg_buff--;
            chman.copyChar.skillPoints++;
        }
            
   
        chman.skillpoints.text = chman.copyChar.skillPoints.ToString();
        chman.dmg_buff.text = chman.copyChar.dmg_buff.ToString();

    }

    public void SetDefBuff(bool added)
    {


        if (added && chman.copyChar.skillPoints > 0)
        {
            chman.copyChar.def_buff++;
            chman.copyChar.skillPoints--;
        }
        else if (!added && chman.copyChar.def_buff > 0)
        {
            chman.copyChar.def_buff--;
            chman.copyChar.skillPoints++;
        }


        chman.skillpoints.text = chman.copyChar.skillPoints.ToString();
        chman.def_buff.text = chman.copyChar.def_buff.ToString();
    }

    public void SetSuppBuff(bool added)
    {


        if (added && chman.copyChar.skillPoints > 0)
        {
            chman.copyChar.supp_buff++;
            chman.copyChar.skillPoints--;
        }
        else if (!added && chman.copyChar.supp_buff > 0)
        {
            chman.copyChar.supp_buff--;
            chman.copyChar.skillPoints++;
        }

        chman.skillpoints.text = chman.copyChar.skillPoints.ToString();
        chman.supp_buff.text = chman.copyChar.supp_buff.ToString();
    }

    public void SetSpeed(bool added)
    {


        if (added && chman.copyChar.skillPoints > 0)
        {
            chman.copyChar.speed++;
            chman.copyChar.skillPoints--;
        }
        else if(!added && chman.copyChar.speed > 0)
        {
            chman.copyChar.speed--;
            chman.copyChar.skillPoints++;
        }


        chman.skillpoints.text = chman.copyChar.skillPoints.ToString();
        chman.speed.text = chman.copyChar.speed.ToString();
    }

}
