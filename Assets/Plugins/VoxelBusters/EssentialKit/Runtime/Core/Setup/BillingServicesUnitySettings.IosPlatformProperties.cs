﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    public partial class BillingServicesUnitySettings
    {
        [Serializable]
        public class IosPlatformProperties
        {
            #region Fields

            [SerializeField]
            private     string          m_customVerificationServerURL;

            #endregion

            #region Properties

            public string CustomVerificationServerURL=> PropertyHelper.GetValueOrDefault(m_customVerificationServerURL);

            #endregion

            #region Constructors

            public IosPlatformProperties(string customVerificationServerURL = null)
            {
                // set properties
                m_customVerificationServerURL     = customVerificationServerURL;
            }

            #endregion
        }
    }
}