using Assets.Utility;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Common.Units {
  [TestClass]
  public class AccountServiceUnitTest {
    #region ctor
    private readonly IAccountService _accountService;
    private readonly RandomMaker _randomMaker;
    private readonly Cryptograph _cryptograph;

    public AccountServiceUnitTest() {
      _accountService = ServiceLocator.Current.GetInstance<IAccountService>();
      _randomMaker = ServiceLocator.Current.GetInstance<RandomMaker>();
      _cryptograph = ServiceLocator.Current.GetInstance<Cryptograph>();
    }
    #endregion

    [TestMethod, TestCategory("AccountService"), TestCategory("Authenticate")]
    public void Authenticate() {
      //var token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQiLCJkZXZpY2VJZCI6IjIiLCJ1c2VybmFtZSI6IjY2NTM0OTgwNyIsImxhc3RTaWduZWRpbkF0IjoiMjAyMC0xMC0xMSAwNjo1MjoxMCBBTSIsIm5iZiI6MTYwMjM5OTEzOSwiZXhwIjoxNjAzMDAzOTM5LCJpYXQiOjE2MDIzOTkxMzksImlzcyI6ImNsaXBib2FyZHkiLCJhdWQiOiJhdWRpZW5jZSJ9.WEYo88TxCAgyECvP3LJiHGl0RKGvbwdXfvs96IZpjyY";
      //var token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQiLCJkZXZpY2VJZCI6IjIiLCJ1c2VybmFtZSI6ImhhbWVkLW1vcmFkaUBsaXZlLmNvbSIsImxhc3RTaWduZWRpbkF0IjoiMjAyMC0xMC0xMSAwNzoxMDoxNiBBTSIsIm5iZiI6MTYwMjQwMDIyMiwiZXhwIjoxNjAzMDA1MDIyLCJpYXQiOjE2MDI0MDAyMjIsImlzcyI6ImNsaXBib2FyZHkiLCJhdWQiOiJhdWRpZW5jZSJ9.W1Mv7UBN0LiEGabVpkhvLBhtDCMO4MPaU7_U1tylYAs";
      //var account = _accountService.AuthenticateAsync(token).GetAwaiter().GetResult();
      //Assert.IsTrue(account != null);
    }

    [TestMethod, TestCategory("AccountService"), TestCategory("First")]
    public void First() {
      //var query = new AccountGetFirstSchema { Id = 4 };
      //var account = _accountService.FirstAsync(query).GetAwaiter().GetResult();
      //Assert.IsTrue(query.StatusCode == 200);
      //Assert.IsTrue(account != null);
    }

    [TestMethod, TestCategory("AccountService"), TestCategory("Add")]
    public void Add() {
      var account = new Account {
        username = "admin",
        provider_id = 1
      };
      var result = _accountService.Add(account);
      Assert.IsNotNull(result);
      //var username = _randomMaker.NewNumber();
      //var duplicated = _accountService.FirstAsync(new AccountGetFirstSchema { Username = username }).GetAwaiter().GetResult();
      //while(duplicated != null) {
      //  username = _randomMaker.NewNumber();
      //  duplicated = _accountService.FirstAsync(new AccountGetFirstSchema { Username = username }).GetAwaiter().GetResult();
      //}

      //var account = new AccountAddSchema {
      //  Password = _cryptograph.RNG("1234"),
      //  ProviderId = AccountProvider.Clipboardy.ToInt(),
      //  Username = username,
      //  CreatedAt = DateTime.Now,
      //  StatusId = Status.Active.ToInt()
      //};
      //var accountId = _accountService.AddAsync(account).GetAwaiter().GetResult();
      //Assert.IsTrue(account.StatusCode == 200);
      //Assert.IsTrue(accountId > 0);
    }

    [TestMethod, TestCategory("AccountService"), TestCategory("Update")]
    public void Update() {
      //var account = new AccountUpdateSchema {
      //  Id = 8,
      //  Password = _cryptograph.RNG("4321"),
      //  LastSignedinAt = DateTime.Now,
      //  StatusId = Status.Active.ToInt()
      //};
      //_accountService.UpdateAsync(account).GetAwaiter().GetResult();
      //Assert.IsTrue(account.StatusCode == 200);
    }

    [TestMethod, TestCategory("AccountService"), TestCategory("Signup")]
    public void Signup() {
      //var user = new SignupBindingModel {
      //  Email = "hamed-moradi@live.com",
      //  Password = "1234",
      //  ConfirmPassword = "1234",
      //  DeviceId = Guid.NewGuid().ToString(),
      //  DeviceName = "DeviceName",
      //  DeviceType = "DeviceType"
      //};
      //var result = _accountService.SignupAsync(user).GetAwaiter().GetResult();

      //Console.WriteLine(result.Code);
      //Console.WriteLine(result.Data);

      //Assert.IsTrue(result.Code == 200);
      //Assert.IsTrue(result.Data != null);
    }

    [TestMethod, TestCategory("AccountService"), TestCategory("ExternalSignup")]
    public void ExternalSignup() {
      //var user = new ExternalUserBindingModel {
      //  MobilePhone = "0924",
      //  GivenName = "GivenName",
      //  Name = "Name",
      //  ProviderId = AccountProvider.Facebook,
      //  NameIdentifier = "NameIdentifier",
      //  Surname = "Surname",
      //  Email = "email_1@clipboardy.com",
      //  //DeviceId = Guid.NewGuid().ToString(),
      //  //DeviceName = "DeviceName",
      //  //DeviceType = "DeviceType"
      //};
      //var result = _accountService.ExternalSignupAsync(user).GetAwaiter().GetResult();

      //Console.WriteLine(result.Code);
      //Console.WriteLine(result.Data);

      //Assert.IsTrue(result.Code == 200);
      //Assert.IsTrue(result.Data != null);
    }

    [TestMethod, TestCategory("AccountService"), TestCategory("Signin")]
    public void Signin() {
      //var user = new SigninBindingModel {
      //  Username = "hamed-moradi@live.com",
      //  Password = "1234"
      //};
      //var result = _accountService.SigninAsync(user).GetAwaiter().GetResult();

      //Console.WriteLine(result.Code);
      //Console.WriteLine(result.Data);

      //Assert.IsTrue(result.Code == 200);
      //Assert.IsTrue(result.Data != null);
    }

    [TestMethod, TestCategory("AccountService"), TestCategory("ExternalSignin")]
    public void ExternalSignin() {
      //var user = new ExternalUserBindingModel {
      //  MobilePhone = "0924",
      //  GivenName = "GivenName",
      //  Name = "Name",
      //  ProviderId = AccountProvider.Facebook,
      //  NameIdentifier = "NameIdentifier",
      //  Surname = "Surname",
      //  Email = "email_1@clipboardy.com",
      //  //DeviceId = "27dc93ce-3190-4dc1-9fd1-b43a4081a16d",
      //  //DeviceName = "DeviceName",
      //  //DeviceType = "DeviceType"
      //};

      //var sccountProfile = new AccountProfileResult {
      //  Id = 3,
      //  AccountId = 21,
      //  LinkedId = "email_1@clipboardy.com",
      //  TypeId = 2,
      //  StatusId = Status.Active
      //};

      //var result = _accountService.ExternalSigninAsync(user, sccountProfile).GetAwaiter().GetResult();

      //Console.WriteLine(result.Code);
      //Console.WriteLine(result.Data);

      //Assert.IsTrue(result.Code == 200);
      //Assert.IsTrue(result.Data != null);
    }
  }
}
