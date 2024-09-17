# VaultPojectUploader

Currently works with Vault Client 2022
Upgrade notes are avaialble below if required

## Running Requirements
* Pull the 'bin' directory to utilize all required .dll's.
* Call the .exe from the root directory and supplying a valid directory string to target files: .\VaultUploader "C:\Users\Name\Box.com\VAULT THESE"
* The code will automatically upload all files within the passed folder location to Vault.
* dll's within the "bin" folder are required for configuration.

## Upgrading SDK (Vault Client Upgrade)
### The adskLicensingSDK dll will need to be updated to match the current version of Vault, the following is only required for upgrades passed Vault 2022
* The SDK exe will need to be run with the new version of Vault which should be found: C:\Program Files\Autodesk\Vault Client 202X\SDK\Setup.exe
* Running the exe will download files to the following location: C:\Program Files\Autodesk\Autodesk Vault 202X SDK
* Download the required dll's from the SDK location: C:\Program Files\Autodesk\Autodesk Vault 202X SDK\bin\x64
* AdskLicensingSDK_5.dll will need to be updated to the latest version
  + Vault 2022: AdskLicensingSDK_5.dll
  + Vault 2023: AdskLicensingSDK_6.dll
  + Vault 2024: AdskLicensingSDK_7.dll, AdskIdentitySDK.dll, AdskIdentitySDK.config
  + Vault 2025 .NET Framework: AdskLicensingSDK_8.dll, AdskIdentitySDK.dll, AdskIdentitySDK.config
* See [Installing the Vault SDK](https://blogs.autodesk.com/vault/2024/03/autodesk-vault-sdk-getting-started-1-installing-the-sdk/) for more details.
