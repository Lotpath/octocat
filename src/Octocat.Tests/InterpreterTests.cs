using Xunit;

namespace Octocat.Tests
{
    public class InterpreterTests
    {
        private readonly Interpreter _interpreter;

        public InterpreterTests()
        {
            _interpreter = new Interpreter();
        }

        [Fact]
        public void should_receive_empty_command_if_input_is_null()
        {
            var command = _interpreter.Interpret(null);
            Assert.True(command.IsEmpty());
        }

        [Fact]
        public void should_receive_empty_command_if_input_is_blank()
        {
            var command = _interpreter.Interpret("");
            Assert.True(command.IsEmpty());
        }

        [Fact]
        public void should_receive_empty_command_if_input_is_whitespace()
        {
            var command = _interpreter.Interpret(" ");
            Assert.True(command.IsEmpty());
        }

        [Fact]
        public void can_extract_verb_from_verb_only_command()
        {
            var command = _interpreter.Interpret("verb");
            Assert.Equal("verb", command.Verb);
            Assert.Null(command.Noun);
        }

        [Fact]
        public void can_extract_verb_and_noun_from_predicate_subject_command()
        {
            var command = _interpreter.Interpret("verb noun");
            Assert.Equal("verb", command.Verb);
            Assert.Equal("noun", command.Noun);
            Assert.Null(command.Organization);
            Assert.Null(command.Repository);
            Assert.Null(command.Target);
        }

        [Fact]
        public void can_extract_four_properties_from_predicate_subject_object_command()
        {
            var command = _interpreter.Interpret("verb noun org/repo");
            Assert.Equal("verb", command.Verb);
            Assert.Equal("noun", command.Noun);
            Assert.Equal("org", command.Organization);
            Assert.Equal("repo", command.Repository);
            Assert.Null(command.Target);
        }

        [Fact]
        public void can_extract_five_properties_from_predicate_subject_object_target_command()
        {
            var command = _interpreter.Interpret("verb noun org/repo target");
            Assert.Equal("verb", command.Verb);
            Assert.Equal("noun", command.Noun);
            Assert.Equal("org", command.Organization);
            Assert.Equal("repo", command.Repository);
            Assert.Equal("target", command.Target);
        }
    }
}