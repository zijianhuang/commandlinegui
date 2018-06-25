// Generated from Arguments.g4 by ANTLR 4.2
import org.antlr.v4.runtime.misc.NotNull;
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link ArgumentsParser}.
 */
public interface ArgumentsListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link ArgumentsParser#init}.
	 * @param ctx the parse tree
	 */
	void enterInit(@NotNull ArgumentsParser.InitContext ctx);
	/**
	 * Exit a parse tree produced by {@link ArgumentsParser#init}.
	 * @param ctx the parse tree
	 */
	void exitInit(@NotNull ArgumentsParser.InitContext ctx);

	/**
	 * Enter a parse tree produced by {@link ArgumentsParser#quotedString}.
	 * @param ctx the parse tree
	 */
	void enterQuotedString(@NotNull ArgumentsParser.QuotedStringContext ctx);
	/**
	 * Exit a parse tree produced by {@link ArgumentsParser#quotedString}.
	 * @param ctx the parse tree
	 */
	void exitQuotedString(@NotNull ArgumentsParser.QuotedStringContext ctx);

	/**
	 * Enter a parse tree produced by {@link ArgumentsParser#parameter}.
	 * @param ctx the parse tree
	 */
	void enterParameter(@NotNull ArgumentsParser.ParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link ArgumentsParser#parameter}.
	 * @param ctx the parse tree
	 */
	void exitParameter(@NotNull ArgumentsParser.ParameterContext ctx);

	/**
	 * Enter a parse tree produced by {@link ArgumentsParser#fixedParameter}.
	 * @param ctx the parse tree
	 */
	void enterFixedParameter(@NotNull ArgumentsParser.FixedParameterContext ctx);
	/**
	 * Exit a parse tree produced by {@link ArgumentsParser#fixedParameter}.
	 * @param ctx the parse tree
	 */
	void exitFixedParameter(@NotNull ArgumentsParser.FixedParameterContext ctx);

	/**
	 * Enter a parse tree produced by {@link ArgumentsParser#parameterName}.
	 * @param ctx the parse tree
	 */
	void enterParameterName(@NotNull ArgumentsParser.ParameterNameContext ctx);
	/**
	 * Exit a parse tree produced by {@link ArgumentsParser#parameterName}.
	 * @param ctx the parse tree
	 */
	void exitParameterName(@NotNull ArgumentsParser.ParameterNameContext ctx);

	/**
	 * Enter a parse tree produced by {@link ArgumentsParser#parameters}.
	 * @param ctx the parse tree
	 */
	void enterParameters(@NotNull ArgumentsParser.ParametersContext ctx);
	/**
	 * Exit a parse tree produced by {@link ArgumentsParser#parameters}.
	 * @param ctx the parse tree
	 */
	void exitParameters(@NotNull ArgumentsParser.ParametersContext ctx);

	/**
	 * Enter a parse tree produced by {@link ArgumentsParser#value}.
	 * @param ctx the parse tree
	 */
	void enterValue(@NotNull ArgumentsParser.ValueContext ctx);
	/**
	 * Exit a parse tree produced by {@link ArgumentsParser#value}.
	 * @param ctx the parse tree
	 */
	void exitValue(@NotNull ArgumentsParser.ValueContext ctx);
}