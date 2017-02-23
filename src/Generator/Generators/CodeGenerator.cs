using System;
using System.Collections.Generic;
using CppSharp.AST;

namespace CppSharp.Generators
{
    public abstract class CodeGenerator : BlockGenerator, IDeclVisitor<bool>
    {
        public BindingContext Context { get; }

        public DriverOptions Options => Context.Options;

        public List<TranslationUnit> TranslationUnits { get; }

        public TranslationUnit TranslationUnit => TranslationUnits[0];

        public abstract string FileExtension { get; }

        public virtual string FilePath =>
            $"{TranslationUnit.FileNameWithoutExtension}.{FileExtension}";

        protected CodeGenerator(BindingContext context, TranslationUnit unit)
            : this(context, new List<TranslationUnit> { unit })
        {
        }

        protected CodeGenerator(BindingContext context, IEnumerable<TranslationUnit> units)
        {
            Context = context;
            TranslationUnits = new List<TranslationUnit>(units);
        }

        public abstract void Process();

        public override string Generate()
        {
            if (Options.IsCSharpGenerator && Options.CompileCode)
                return base.GenerateUnformatted();

            return base.Generate();
        }

        public void GenerateMultiLineComment(List<string> lines, RawCommentKind kind)
        {
            var lineCommentPrologue = Comment.GetLineCommentPrologue(kind);
            if (!string.IsNullOrWhiteSpace(lineCommentPrologue))
                WriteLine("{0}", lineCommentPrologue);

            var multiLineCommentPrologue = Comment.GetMultiLineCommentPrologue(kind);
            foreach (var line in lines)
                WriteLine("{0} {1}", multiLineCommentPrologue, line);

            var lineCommentEpilogue = Comment.GetLineCommentEpilogue(kind);
            if (!string.IsNullOrWhiteSpace(lineCommentEpilogue))
                WriteLine("{0}", lineCommentEpilogue);
        }

        public virtual void GenerateFilePreamble(RawCommentKind kind)
        {
            var lines = new List<string>
            {
                "----------------------------------------------------------------------------",
                "<auto-generated>",
                "This is autogenerated code by CppSharp.",
                "Do not edit this file or all your changes will be lost after re-generation.",
                "</auto-generated>",
                "----------------------------------------------------------------------------"
            };

            PushBlock(BlockKind.Header);
            GenerateMultiLineComment(lines, kind);
            PopBlock();
        }

        #region Visitor methods

        public virtual bool VisitDeclaration(Declaration decl)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitTranslationUnit(TranslationUnit unit)
        {
            return VisitDeclContext(unit);
        }

        public virtual bool VisitDeclContext(DeclarationContext context)
        {
            foreach (var decl in context.Declarations)
                if (!decl.IsGenerated)
                    decl.Visit(this);

            return true;
        }

        public virtual bool VisitClassDecl(Class @class)
        {
            return VisitDeclContext(@class);
        }

        public virtual bool VisitFieldDecl(Field field)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitFunctionDecl(Function function)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitMethodDecl(Method method)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitParameterDecl(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitTypedefNameDecl(TypedefNameDecl typedef)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitTypedefDecl(TypedefDecl typedef)
        {
            return VisitTypedefNameDecl(typedef);
        }

        public virtual bool VisitTypeAliasDecl(TypeAlias typeAlias)
        {
            return VisitTypedefNameDecl(typeAlias);
        }

        public virtual bool VisitEnumDecl(Enumeration @enum)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitEnumItemDecl(Enumeration.Item item)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitVariableDecl(Variable variable)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitMacroDefinition(MacroDefinition macro)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitNamespace(Namespace @namespace)
        {
            return VisitDeclContext(@namespace);
        }

        public virtual bool VisitEvent(Event @event)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitProperty(Property property)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitFriend(Friend friend)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitClassTemplateDecl(ClassTemplate template)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitClassTemplateSpecializationDecl(ClassTemplateSpecialization specialization)
        {
            return VisitClassDecl(specialization);
        }

        public virtual bool VisitFunctionTemplateDecl(FunctionTemplate template)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitFunctionTemplateSpecializationDecl(FunctionTemplateSpecialization specialization)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitVarTemplateDecl(VarTemplate template)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitVarTemplateSpecializationDecl(VarTemplateSpecialization template)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitTemplateTemplateParameterDecl(TemplateTemplateParameter templateTemplateParameter)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitTemplateParameterDecl(TypeTemplateParameter templateParameter)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitNonTypeTemplateParameterDecl(NonTypeTemplateParameter nonTypeTemplateParameter)
        {
            throw new NotImplementedException();
        }

        public virtual bool VisitTypeAliasTemplateDecl(TypeAliasTemplate typeAliasTemplate)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
