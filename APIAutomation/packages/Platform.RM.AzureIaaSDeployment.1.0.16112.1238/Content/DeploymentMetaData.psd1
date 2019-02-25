 @{
    NonEnvironmentData = @{
        Subscription = "MSI-Development-Enterprise";
        StorageAccount = "buildcontrolled";
        EnterpriseSystemName = "RMSample";
        BuildNumber = "[BuildNumber]";
    };
 
    Envioronment = @(
            @{
                Name = "IN1";
                Subscription = "Ent-NonProd-Workload-01";
                CloudService = @(
                        @{
                            Name = "gze-umb-in1-005";
                            Servers = @(
                                            @{
                                                Name = "GZEDUCKIN1EXP01";
                                                DSCConfigurationFileName = "NonProd.RMConfiguration.ps1";
                                                DSCConfigurationDataFileName = "NonProd.INTG.Server.ConfigData.psd1";
                                                DSCConfigurationName = "RMConfiguration";
                                             },
								            @{
                                                Name = "GZEDUCKIN1EXP02";
                                                DSCConfigurationFileName = "NonProd.RMConfiguration.ps1";
                                                DSCConfigurationDataFileName = "NonProd.INTG.Server.ConfigData.psd1";
                                                DSCConfigurationName = "RMConfiguration";
                                             }
                                       );
                           }
                        );
            },
		   @{
                Name = "IN1TEST";
                Subscription = "Ent-NonProd-Workload-01";
                CloudService = @(
                        @{
                            Name = "gze-umb-np1-001";
                            Servers = @(
                                            @{
                                                Name = "GZECORENP1MTM02";
                                                DSCConfigurationFileName = "NonProd.TestConfiguration.ps1";
                                                DSCConfigurationDataFileName = "NonProd.IN1TEST.Server.ConfigData.psd1";
                                                DSCConfigurationName = "TestConfiguration";         
                                             }
                                       );
                           }
                        );
            },
		   @{
                Name = "LT1TEST";
                Subscription = "Ent-NonProd-Workload-01";
                CloudService = @(
                        @{
                            Name = "gze-umb-np1-001";
                            Servers = @(
                                            @{
                                                Name = "GZECORENP1MTM02";
                                                DSCConfigurationFileName = "NonProd.TestConfiguration.ps1";
                                                DSCConfigurationDataFileName = "NonProd.LT1TEST.Server.ConfigData.psd1";
                                                DSCConfigurationName = "TestConfiguration";         
                                             }
                                       );
                           }
                        );
            },
		   @{
                Name = "IN2TEST";
                Subscription = "Ent-NonProd-Workload-01";
                CloudService = @(
                        @{
                            Name = "gze-umb-np1-001";
                            Servers = @(
                                            @{
                                                Name = "GZECORENP1MTM02";
                                                DSCConfigurationFileName = "NonProd.TestConfiguration.ps1";
                                                DSCConfigurationDataFileName = "NonProd.IN2TEST.MTM.ConfigData.psd1";
                                                DSCConfigurationName = "TestConfiguration";         
                                             }
                                       );
                           }
                        );
            }
        );
  }
