using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Translate;
using System.Text.RegularExpressions;

var blockComments = @"/\*(.*?)\*/";
var lineComments = @"//(.*?)\r?\n";
var chain = new CredentialProfileStoreChain();
AWSCredentials awsCredentials;

if (chain.TryGetAWSCredentials("YOUR_PROFILE_NAME", out awsCredentials))
{
    using var client = new AmazonTranslateClient(awsCredentials, RegionEndpoint.EUWest1);
    var translator = new CommentTranslator.Translator(client);

    string rootfolder = @"TARGET_FOLDER_PATH";
    string[] files = Directory.GetFiles(rootfolder, "*.cs", SearchOption.AllDirectories);
    foreach (string file in files)
    {
        try
        {
            string contents = File.ReadAllText(file);
            // check if comment exists
            if (!contents.Contains("//"))
            {
                continue;
            }

            // replace if comment is not in english
            var newContents = Regex.Replace(contents, blockComments + "|" + lineComments, me =>
            {
                return translator.ToEn(me.Value).Result.TranslatedText;
            }, RegexOptions.Singleline);

            if (contents != newContents)
            {
                File.WriteAllText(file, newContents);
            } 
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    Console.WriteLine("Translation finished");
}
else
{
    Console.WriteLine("Failed to get AWS credentials");
}
