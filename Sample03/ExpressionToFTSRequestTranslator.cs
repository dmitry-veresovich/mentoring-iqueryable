using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sample03
{
	public class ExpressionToFTSRequestTranslator : ExpressionVisitor
	{
		StringBuilder resultString;

		public string Translate(Expression exp)
		{
			resultString = new StringBuilder();
			Visit(exp);

			return resultString.ToString();
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof(Queryable)
				&& node.Method.Name == "Where")
			{
				var predicate = node.Arguments[1];
				Visit(predicate);

				return node;
			}
			if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "StartsWith")
			{
                Visit(node.Object);
                resultString.Append("(");
                var arg = node.Arguments[0];
                Visit(arg);
                resultString.Append("*)");
			    return node;
			}
            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "EndsWith")
            {
                Visit(node.Object);
                resultString.Append("(*");
                var arg = node.Arguments[0];
                Visit(arg);
                resultString.Append(")");
                return node;
            }
            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "Contains")
            {
                Visit(node.Object);
                resultString.Append("(*");
                var arg = node.Arguments[0];
                Visit(arg);
                resultString.Append("*)");
                return node;
            }
            return base.VisitMethodCall(node);
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.Equal:
			        Expression memberAccessExpression;
			        Expression constantExpression;

			        if (node.Left.NodeType == ExpressionType.MemberAccess)
			        {
			            memberAccessExpression = node.Left;
			            if (node.Right.NodeType == ExpressionType.Constant)
			            {
			                constantExpression = node.Right;
			            }
			            else
			            {
                            throw new NotSupportedException(string.Format("Right operand should be constant", node.NodeType));
                        }
			        }
			        else if (node.Left.NodeType == ExpressionType.Constant)
			        {
                        constantExpression = node.Left;
                        if (node.Right.NodeType == ExpressionType.MemberAccess)
                        {
                            memberAccessExpression = node.Right;
                        }
                        else
                        {
                            throw new NotSupportedException(string.Format("Right operand should be property or field", node.NodeType));
                        }
                    }
			        else
			        {
                        throw new NotSupportedException(string.Format("Left operand should be constant, property or field", node.NodeType));
                    }

					Visit(memberAccessExpression);
					resultString.Append("(");
					Visit(constantExpression);
					resultString.Append(")");
					break;

                case ExpressionType.AndAlso:
			        Visit(node.Left);
			        resultString.Append(" ");
			        Visit(node.Right);
                    break;
				default:
					throw new NotSupportedException(string.Format("Operation {0} is not supported", node.NodeType));
			};

			return node;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			resultString.Append(node.Member.Name).Append(":");

			return base.VisitMember(node);
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			resultString.Append(node.Value);

			return node;
		}
	}
}
