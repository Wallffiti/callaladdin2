﻿using CallAladdin.Model;
using CallAladdin.Services;
using CallAladdin.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CallAladdin.ViewModel
{
    public class JobHistoryViewViewModel : BaseViewModel
    {
        private IUserService userService;
        private UserProfile contractorProfile;
        private JobViewCommonUserControlViewModel jobviewCommonUserControlViewModel;
        private BaseViewModel parentViewModel;
        private string contractorAvatarImage;
        private string contractorName;
        private DateTime createdDate;
        private bool hasContractor;
        private Job selectedJob;

        public UserProfile GetContractorProfile()
        {
            return contractorProfile;
        }

        public Job Job
        {
            get
            {
                return selectedJob;
            }
            set
            {
                selectedJob = value;
                OnPropertyChanged(nameof(Job));
            }
        }

        public JobViewCommonUserControlViewModel JobViewCommonUserControlViewModel
        {
            get
            {
                return jobviewCommonUserControlViewModel;
            }
            set
            {
                jobviewCommonUserControlViewModel = value;
                OnPropertyChanged(nameof(JobViewCommonUserControlViewModel));
            }
        }

        public string ContractorAvatarImage
        {
            get
            {
                return contractorAvatarImage;
            }
            set
            {
                contractorAvatarImage = value;
                OnPropertyChanged(nameof(ContractorAvatarImage));
            }
        }

        public string ContractorName
        {
            get
            {
                return contractorName;
            }
            set
            {
                contractorName = value;
                OnPropertyChanged(nameof(ContractorName));
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                createdDate = value;
                OnPropertyChanged(nameof(CreatedDate));
            }
        }

        public bool HasContractor
        {
            get
            {
                return hasContractor;
            }
            set
            {
                hasContractor = value;
                OnPropertyChanged(nameof(HasContractor));
            }
        }

        public ICommand GoToContractorProfileCmd { get; set; }

        public JobHistoryViewViewModel(object owner)
        {
            parentViewModel = (BaseViewModel)owner;

            if (parentViewModel is HistoryUserControlViewModel)
            {
                var historyViewModel = (HistoryUserControlViewModel)parentViewModel;
                this.Job = historyViewModel.GetSelectedJob();
                var contractorSystemUUID = this.Job?.ContractorSystemUUID;  

                //TODO: need to distinguish whether user is requestor or contractor, hence the system UUID here would be different
                if (!string.IsNullOrEmpty(contractorSystemUUID))
                {
                    HasContractor = true;
                    userService = new UserService();
                    GetContractorProfile(contractorSystemUUID);
                    GoToContractorProfileCmd = new Xamarin.Forms.Command(async e =>
                    {
                        await Navigator.Instance.NavigateTo(PageType.CONTRACTOR_PROFILE_VIEW, this); //TODO: need to make this more generic where profile view can be contractor OR requestor
                    },
                    param =>
                    {
                        return true;
                    });
                }
                else
                {
                    HasContractor = false;
                }
            }

            jobviewCommonUserControlViewModel = new JobViewCommonUserControlViewModel(this);
        }

        private void GetContractorProfile(string contractorSystemUUID)
        {
            Task.Run(async () =>
            {
                //jobviewCommonUserControlViewModel.IsBusy = true;
                contractorProfile = await userService.GetUserProfileByUUID(contractorSystemUUID);
                if (contractorProfile != null)
                {
                    this.ContractorAvatarImage = contractorProfile.PathToProfileImage;
                    this.ContractorName = contractorProfile.Name;
                    this.CreatedDate = contractorProfile.CreatedDate;
                }
                //jobviewCommonUserControlViewModel.IsBusy = false;
            });
        }
    }
}
