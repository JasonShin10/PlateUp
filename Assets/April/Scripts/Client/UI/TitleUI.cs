using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class TitleUI : UIBase
    {
        public void OnClickHideUI()
        {
            UIManager.Hide<TitleUI>(UIList.TitleUI);
        }
    }
}