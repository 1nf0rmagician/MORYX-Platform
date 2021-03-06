// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Moryx.Workflows;
using Moryx.Workflows.Transitions;
using Moryx.Workflows.WorkplanSteps;

namespace Moryx.Tests.Workflows
{
    [DataContract]
    public class DummyStep : WorkplanStepBase
    {
        private DummyStep()
        {

        }

        public DummyStep(int outputs)
            : this(outputs, "DummyStep")
        {
        }

        public DummyStep(int outputs, string name)
        {
            Outputs = new IConnector[outputs];
            _name = name;
        }

        [DataMember]
        private readonly string _name;

        ///
        public override string Name => _name;

        [EditorBrowsable]
        public int Number { get; set; }

        ///
        protected override TransitionBase Instantiate(IWorkplanContext context)
        {
            return new DummyTransition { Context = context, Name = Name };
        }
    }

    public class DummyTransition : TransitionBase, IObservableTransition
    {
        public string Name { get; set; }

        public IWorkplanContext Context { get; set; }

        public int ResultOutput { get; set; }

        ///
        protected override void InputTokenAdded(object sender, IToken token)
        {
            ((IPlace)sender).Remove(token);
            StoredTokens.Add(token);
            Triggered(this, new EventArgs());
            if (ResultOutput >= 0) // Resume directly
                PlaceToken(Outputs[ResultOutput], StoredTokens.First());
        }

        public override void Resume()
        {
            if (StoredTokens.Any())
                Triggered(this, new EventArgs());
        }

        public void ResumeAsync(int result)
        {
            PlaceToken(Outputs[result], StoredTokens.First());
        }

        public event EventHandler Triggered;
    }
}
