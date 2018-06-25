using System;
namespace Fonlow.CommandLineGui
{
    public interface ICommand
    {
        string CommandLine { get; }
        string CommandName { get; }
        object ParametersAndOptionsProxy { get; }
        string GetParameterDescription(string parameterName);
        string DefinedParametersAndOptions { get; }
        bool ParseCommandLine(string commandLine);
        string PickupParameterAtPosition(string commandText, int position);

        event EventHandler<TextMessageEventArgs> ReportErrorHandler;

    }

    /// <summary>
    /// Options of a command line programs might be constrainted by some rules of inclusion or exclusion. After a user
    /// had defined some options which could be contrditing to the rules, the program should be able to refine some optons.
    /// </summary>
    public interface IRefineOptions
    {
        /// <summary>
        /// Refine some options.
        /// </summary>
        void Refine();

        /// <summary>
        /// Validate the validity of options, and throw exception there exist critically invalid options.
        /// </summary>
        /// <returns>Error message. If not OK, InvalidParametersException is thrown.</returns>
        void Validate();
    }
}
