using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagersController : Patterns.Singleton<ManagersController>
{
    public List<ManagerDataSO> managerDataSOs => _managerDataSOList;
    private List<ManagerDataSO> _managerDataSOList => MainGameData.managerDataSOList;
}
